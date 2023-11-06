using Microsoft.Extensions.Configuration;

namespace GptWrapper.Classes;

public interface IActionService
{
    List<ActionTemplate> GetActionsFromConfig(IConfiguration configuration);
    Task PerfomKeyAction(Keys key);
}

public class ActionService : IActionService
{
    private IWebViewService _webViewWorker;
    private IWindowManagementService _windowManagement;

    private List<ActionTemplate> _actions;

    public ActionService(IWebViewService webViewWorker, IWindowManagementService windowManagement)
    {
        _webViewWorker = webViewWorker;
        _windowManagement = windowManagement;
    }

    public List<ActionTemplate> GetActionsFromConfig(IConfiguration configuration)
    {
        _actions = new List<ActionTemplate>();
        configuration.GetSection("Actions").Bind(_actions);

        return _actions;
    }

    public async Task PerfomKeyAction(Keys key)
    {
        var keyString = key.ToString();
        var action = _actions.FirstOrDefault(x => x.Key == keyString);
        if (action == null) return;
        await PerformAction(action);
    }

    private async Task PerformAction(ActionTemplate action)
    {
        var refactorText = await GetTemplateWithFocusedText(action.Template);
        if (string.IsNullOrEmpty(refactorText)) return;

        var index = ProcessRefactorText(ref refactorText);
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

    private static async Task<string> GetTemplateWithFocusedText(string template)
    {
        var text = await GetFocusedText();
        var askText = template.Replace("@focusedText", text);
        return askText;
    }

    private static async Task<string> GetFocusedText()
    {
        Clipboard.Clear();

        await Task.Delay(150); // For copy to work those delays has to be that long.
        SendKeys.Send("^{c}");
        await Task.Delay(333);

        return Clipboard.GetText();
    }

    private static int ProcessRefactorText(ref string refactorText)
    {
        const string searchString = "@returnPoint";
        int index = -1;
        if (refactorText.Contains(searchString))
        {
            index = refactorText.IndexOf(searchString);
            refactorText = refactorText.Replace(searchString, string.Empty);
        }
        return index;
    }
}
