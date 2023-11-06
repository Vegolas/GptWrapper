using WindowsInput;

namespace GptWrapper.Classes;

public interface IWindowManagementService
{
    Task SendToGptForm(string refactorText);
    Task SetStartupPosition();
    Task SwitchFocusWindow();
}

public class WindowManagementService : IWindowManagementService
{
    private readonly Form _window;
    private readonly IWebViewService _webViewWorker;
    private readonly InputSimulator _inputSimulator;

    private nint _lastActiveWindow;

    public WindowManagementService(Form window, IWebViewService viewWorker, InputSimulator inputSimulator)
    {
        _window = window;
        _webViewWorker = viewWorker;
        _inputSimulator = inputSimulator;
    }

    public async Task SetStartupPosition()
    {
        _window.BringToFront();
        _window.Activate();

        _inputSimulator.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.LWIN);
        _inputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RIGHT);
        _inputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.LWIN);

        await Task.Delay(250);

        _inputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.ESCAPE);

        _window.Left += 200;
        _window.Width -= 200;
    }

    public async Task SwitchFocusWindow()
    {
        if (DllImportWrapper.ApplicationIsActivated() && _lastActiveWindow > 0)
        {
            DllImportWrapper.SetForegroundWindow(_lastActiveWindow);
        }
        else
        {
            _lastActiveWindow = DllImportWrapper.GetForegroundWindow();
            await BringToFrontAndFocusInput();
        }
    }

    public async Task SendToGptForm(string refactorText)
    {
        await _webViewWorker.SetInputBoxText(refactorText);

        _window.BringToFront();
        _window.Activate();

        await _webViewWorker.FocusInputBox();

        _inputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.SPACE); // Needed for GPT input box to resize after 
        _inputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.BACK); // recieving new text.
    }

    private async Task BringToFrontAndFocusInput()
    {
        if (_window.WindowState == FormWindowState.Minimized)
        {
            _window.WindowState = FormWindowState.Normal;
            await Task.Delay(333);
            await SetStartupPosition();
        }
        _window.BringToFront();
        _window.Activate();
        await _webViewWorker.FocusInputBox();
    }
}
