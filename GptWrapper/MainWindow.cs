using GptWrapper.Classes;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace GptWrapper;

public partial class MainWindow : Form
{
    private const string SETTINGS_PATH = "settings.json";

    private KeyboardHook _keyboardHook;
    private WindowsInput.InputSimulator _inputSimulator;
    private IConfiguration _config;
    private List<ActionTemplate> _actions;
    private WebViewService _webViewWorker;
    private WindowManagementService _windowManagement;
    
    public MainWindow()
    {
        InitializeComponent();
        _inputSimulator = new WindowsInput.InputSimulator();
        _webViewWorker = new WebViewService(WebView);
        _windowManagement = new WindowManagementService(this, _webViewWorker, _inputSimulator);
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        this.Icon = new Icon("Resources\\openai-logo.ico");
        LoadConfiguration();
        PrepareKeyboardHooks();
        await _webViewWorker.PrepareWebView();
        await _windowManagement.SetPosition();
    }

    private void LoadConfiguration()
    {
        var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile(SETTINGS_PATH, optional: false, reloadOnChange: true);

        _config = builder.Build();
    }

    private void PrepareKeyboardHooks()
    {
        _keyboardHook = new KeyboardHook();
        _actions = new List<ActionTemplate>();

        _keyboardHook.KeyPressed += new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);

        var firstKey = Enum.Parse<ModifierKeys>(_config["FirstKey"]);
        var secondKey = Enum.Parse<ModifierKeys>(_config["SecondKey"]);

        _keyboardHook.RegisterHotKey(firstKey | secondKey, Enum.Parse<Keys>(_config["FocusKey"]));
        _keyboardHook.RegisterHotKey(firstKey | secondKey, Enum.Parse<Keys>(_config["SubmitKey"]));

        _config.GetSection("Actions").Bind(_actions);

        foreach (var action in _actions)
        {
            _keyboardHook.RegisterHotKey(firstKey | secondKey, Enum.Parse<Keys>(action.Key));
            Debug.WriteLine($"Registred action: {action.Name} ({firstKey} + {secondKey} + {action.Key})");
        }
    }

    private async Task PerformAction(ActionTemplate action)
    {
        var refactorText = await Helpers.GetTemplateWithFocusedText(action.Template);
        if (string.IsNullOrEmpty(refactorText)) return;

        var index = Helpers.ProcessRefactorText(ref refactorText);
        await _windowManagement.SendToGptForm(refactorText);

        if (action.AutoSubmit)
        {
            await _webViewWorker.SubmitMessage();
        }
        else if (index >= 0)
        {
            await _webViewWorker.SetCursorInTextArea(index);
        }
    }

    private void ReloadKeys()
    {
        DisposeKeyboardHooks();
        PrepareKeyboardHooks();
    }

    private void DisposeKeyboardHooks()
    {
        _keyboardHook.KeyPressed -= hook_KeyPressed;
        _keyboardHook.Dispose();
    }

    private void MainWindow_SizeChanged(object sender, EventArgs e)
    {
        settingsPanel.Location = new Point(12, 41);
        settingsPanel.Size = new Size(this.Width - 40, this.Height - 150);
        settingsButton.Location = new Point(this.Width - 120, 12);
        settingsBox.Text = File.ReadAllText(SETTINGS_PATH);
    }

    private void button1_Click(object sender, EventArgs e)
    {
        if (settingsPanel.Visible && Helpers.SaveSettings(settingsBox.Text, SETTINGS_PATH)) // So it's closing right now
        {
            LoadConfiguration();
            ReloadKeys();
            settingsPanel.Visible = !settingsPanel.Visible;
        }
        else if (settingsBox.Visible == false)
        {
            settingsPanel.Visible = !settingsPanel.Visible;
        }
    }

    async void hook_KeyPressed(object sender, KeyPressedEventArgs e)
    {
        if (e.Key == Enum.Parse<Keys>(_config["FocusKey"]))
        {
            await _windowManagement.SwitchFocusWindow();
        }
        else if (e.Key == Keys.Enter)
        {
            await _webViewWorker.SubmitMessage();
        }
        else
        {
            var keyString = e.Key.ToString();
            var action = _actions.FirstOrDefault(x => x.Key == keyString);
            if (action == null) return;
            await PerformAction(action);
        }
    }
}
