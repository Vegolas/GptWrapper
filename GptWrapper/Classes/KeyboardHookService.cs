using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace GptWrapper.Classes;

public interface IKeyboardHookService
{
    void Init();
    void ReloadConfiguration(IConfiguration newConfig);
}

public class KeyboardHookService : IDisposable, IKeyboardHookService
{
    private IConfiguration _config;
    private IWebViewService _webViewWorker;
    private IWindowManagementService _windowManagement;

    private IActionService _actionService;
    private KeyboardHook _keyboardHook;

    private ModifierKeys _firstKey;
    private ModifierKeys _secondKey;

    private Keys _submitKey;
    private Keys _focusKey;

    public KeyboardHookService(IConfiguration configuration, IWindowManagementService windowManagement, IWebViewService webViewWorker)
    {
        _config = configuration;
        _windowManagement = windowManagement;
        _webViewWorker = webViewWorker;
        _actionService = new ActionService(_webViewWorker, _windowManagement);
    }

    public void Init()
    {
        _keyboardHook = new KeyboardHook();
        _keyboardHook.KeyPressed += new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);

        GetBaseKeys();

        _keyboardHook.RegisterHotKey(_firstKey | _secondKey, _focusKey);
        _keyboardHook.RegisterHotKey(_firstKey | _secondKey, _submitKey);

        HookActions(_actionService.GetActionsFromConfig(_config));
    }

    public void ReloadConfiguration(IConfiguration newConfig)
    {
        _config = newConfig;
        DisposeKeyboardHooks();
        Init();
    }

    private void GetBaseKeys()
    {
        _firstKey = Enum.Parse<ModifierKeys>(_config["FirstKey"]);
        _secondKey = Enum.Parse<ModifierKeys>(_config["SecondKey"]);
        _focusKey = Enum.Parse<Keys>(_config["FocusKey"]);
        _submitKey = Enum.Parse<Keys>(_config["SubmitKey"]);
    }

    private void HookActions(List<ActionTemplate> actions)
    {
        foreach (var action in actions)
        {
            _keyboardHook.RegisterHotKey(_firstKey | _secondKey, Enum.Parse<Keys>(action.Key));
            Debug.WriteLine($"Registred action: {action.Name} ({_firstKey} + {_secondKey} + {action.Key})");
        }
    }

    private void DisposeKeyboardHooks()
    {
        _keyboardHook.KeyPressed -= hook_KeyPressed;
        _keyboardHook.Dispose();
    }

    private async void hook_KeyPressed(object sender, KeyPressedEventArgs e)
    {
        if (e.Key == _focusKey)
        {
            await _windowManagement.SwitchFocusWindow();
        }
        else if (e.Key == _submitKey)
        {
            await _webViewWorker.SubmitMessage();
        }
        else
        {
            await _actionService.PerfomKeyAction(e.Key);
        }
    }

    public void Dispose()
    {
        _keyboardHook.Dispose();
    }
}
