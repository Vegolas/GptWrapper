using System.Web;

namespace GptWrapper;

public partial class MainWindow : Form
{
    KeyboardHook hook = new KeyboardHook();
    WindowsInput.InputSimulator iss = new WindowsInput.InputSimulator();
    nint LastActiveWindow;

    public MainWindow()
    {
        InitializeComponent();
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        PrepareKeyboardHooks();
        await PrepareWebView();
        await SetPosition();
    }

    private void PrepareKeyboardHooks()
    {
        hook.KeyPressed +=
            new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);

        var firstKey = GptWrapper.ModifierKeys.Control;
        var secondKey = GptWrapper.ModifierKeys.Shift;

        hook.RegisterHotKey(firstKey | secondKey, Keys.W);

        hook.RegisterHotKey(firstKey | secondKey, Keys.R);

        hook.RegisterHotKey(firstKey | secondKey, Keys.C);

        hook.RegisterHotKey(firstKey | secondKey, Keys.E);

        hook.RegisterHotKey(firstKey | secondKey, Keys.Enter);
    }

    private async Task PrepareWebView()
    {
        var dataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GptWrapper");
        var env = await Microsoft.Web.WebView2.Core.CoreWebView2Environment.CreateAsync(null, dataFolder, null);
        await WebView.EnsureCoreWebView2Async(env);
        WebView.CoreWebView2.Navigate("https://chat.openai.com/");
        await Task.Delay(1000);
    }

    private async Task SetPosition()
    {
        this.BringToFront();
        this.Activate();

        iss.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.LWIN);
        iss.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RIGHT);
        iss.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.LWIN);

        await Task.Delay(250);

        iss.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.ESCAPE);

        this.Left += 200;
        this.Width -= 200;
    }

    async void hook_KeyPressed(object sender, KeyPressedEventArgs e)
    {
        if (e.Key == Keys.R)
        {
            var text = await GetFocusedText();
            var refactorText = FormatMessage(text, MessageType.Refactor);
            if (string.IsNullOrEmpty(refactorText) == false)
            {
                await SendToGptForm(refactorText);
                await SubmitMessage();
            }
        }
        else if (e.Key == Keys.W)
        {
            if (DllImportWrapper.ApplicationIsActivated() && LastActiveWindow > 0)
            {
                DllImportWrapper.SetForegroundWindow(LastActiveWindow);
            }
            else
            {
                LastActiveWindow = DllImportWrapper.GetForegroundWindow();
                await BringToFrontAndFocusInput();
            }
        }
        else if (e.Key == Keys.C)
        {
            var text = await GetFocusedText();
            var askText = FormatMessage(text, MessageType.Ask);
            if (string.IsNullOrEmpty(askText) == false)
            {
                await SendToGptForm(askText);
                await WebView.ExecuteScriptAsync($"document.getElementById('prompt-textarea').focus();");
                await WebView.ExecuteScriptAsync($"document.getElementById('prompt-textarea').setSelectionRange(0,0);");
            }
        }
        else if (e.Key == Keys.E)
        {
            var text = await GetFocusedText();
            var errorText = FormatMessage(text, MessageType.Error);
            if (string.IsNullOrEmpty(errorText) == false)
            {
                await SendToGptForm(errorText);
                await SubmitMessage();
            }
        }
        else if (e.Key == Keys.Enter)
        {
            await SubmitMessage();
        }
    }

    private async Task BringToFrontAndFocusInput()
    {
        if (WindowState == FormWindowState.Minimized)
        {
            WindowState = FormWindowState.Normal;
            await Task.Delay(333);
            await SetPosition();
        }
        this.BringToFront();
        this.Activate();
        await WebView.ExecuteScriptAsync($"document.getElementById('prompt-textarea').focus();");
    }

    private async Task SendToGptForm(string refactorText)
    {
        var escapedText = HttpUtility.JavaScriptStringEncode(refactorText);
        await WebView.ExecuteScriptAsync($"document.getElementById('prompt-textarea').value = '{escapedText}'");

        this.BringToFront();
        this.Activate();
        WebView.Focus();
        await WebView.ExecuteScriptAsync($"document.getElementById('prompt-textarea').focus();");
        iss.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.SPACE);
    }

    private async Task SubmitMessage()
    {
        await Task.Delay(333);
        await WebView.ExecuteScriptAsync($"document.querySelector('[data-testid=send-button]').click();");
        await Task.Delay(333);
    }

    private string FormatMessage(string text, MessageType type)
    {
        text = text.Replace('`', '\'');

        if (string.IsNullOrEmpty(text))
            return string.Empty;

        string template = type switch
        {
            MessageType.Ask => $"\r\n<context>\r\n{text}\r\n</context>",
            MessageType.Refactor => $"Refactor this code:\r\n<code>\r\n{text}\r\n</code>",
            MessageType.Error => $"Im encountering this error:\r\n<error>\r\n{text}\r\n</error>",
            _ => text
        };

        return template;
    }


    private async Task<string> GetFocusedText()
    {
        Clipboard.Clear();

        await Task.Delay(500);
        SendKeys.Send("^{c}");
        await Task.Delay(250);

        return Clipboard.GetText();
    }
}
