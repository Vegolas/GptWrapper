using Microsoft.Web.WebView2.WinForms;
using System.Web;

namespace GptWrapper.Classes;
public class WebViewService
{
    private readonly WebView2 _webView;

    public WebViewService(WebView2 webView)
    {
        _webView = webView;
    }

    public async Task PrepareWebView()
    {
        var dataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GptWrapper");
        var env = await Microsoft.Web.WebView2.Core.CoreWebView2Environment.CreateAsync(null, dataFolder, null);
        await _webView.EnsureCoreWebView2Async(env);
        _webView.CoreWebView2.Navigate("https://chat.openai.com/");
        await Task.Delay(1000);
    }

    public async Task SubmitMessage()
    {
        await Task.Delay(100);
        await _webView.ExecuteScriptAsync($"document.querySelector('[data-testid=send-button]').click();");
    }

    public async Task SetCursorInTextArea(int index)
    {
        const string scriptTemplate = "document.getElementById('prompt-textarea').{0};";
        await _webView.ExecuteScriptAsync(string.Format(scriptTemplate, "focus()"));
        await _webView.ExecuteScriptAsync(string.Format(scriptTemplate, $"setSelectionRange({index},{index})"));
    }

    public async Task FocusInputBox()
    {
        _webView.Focus();
        await _webView.ExecuteScriptAsync($"document.getElementById('prompt-textarea').focus();");
    }

    public async Task SetInputBoxText(string refactorText)
    {
        var escapedText = HttpUtility.JavaScriptStringEncode(refactorText);
        await _webView.ExecuteScriptAsync($"document.getElementById('prompt-textarea').value = '{escapedText}'");
    }
}
