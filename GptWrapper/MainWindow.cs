using GptWrapper.Classes;
using Microsoft.Extensions.Configuration;

namespace GptWrapper;

public partial class MainWindow : Form
{
    private const string SETTINGS_PATH = "settings.json";

    private WindowsInput.InputSimulator _inputSimulator;
    private IConfiguration _config;
    private IWebViewService _webViewWorker;
    private IWindowManagementService _windowManagement;
    private IKeyboardHookService _keyboardHookService;

    public MainWindow()
    {
        InitializeComponent();
        _config = LoadConfiguration();
        _inputSimulator = new WindowsInput.InputSimulator();
        _webViewWorker = new WebViewService(WebView);
        _windowManagement = new WindowManagementService(this, _webViewWorker, _inputSimulator);
        _keyboardHookService = new KeyboardHookService(_config, _windowManagement, _webViewWorker);
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        this.Icon = new Icon("Resources\\openai-logo.ico");

        _keyboardHookService.Init();

        await _webViewWorker.PrepareWebView();
        await _windowManagement.SetStartupPosition();
    }

    private IConfiguration LoadConfiguration()
    {
        var builder = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile(SETTINGS_PATH, optional: false, reloadOnChange: true);

        return builder.Build();
    }

    private void MainWindow_SizeChanged(object sender, EventArgs e)
    {
        settingsPanel.Location = new Point(12, 41);
        settingsPanel.Size = new Size(this.Width - 40, this.Height - 170);
        settingsButton.Location = new Point(this.Width - 120, 0);
        settingsBox.Text = File.ReadAllText(SETTINGS_PATH);
    }

    private void button1_Click(object sender, EventArgs e)
    {
        if (settingsPanel.Visible && Helpers.SaveSettings(settingsBox.Text, SETTINGS_PATH)) // So it's closing right now
        {
            _config = LoadConfiguration();
            _keyboardHookService.ReloadConfiguration(_config);
            settingsPanel.Visible = !settingsPanel.Visible;
        }
        else if (settingsBox.Visible == false)
        {
            settingsPanel.Visible = !settingsPanel.Visible;
        }
    }
}