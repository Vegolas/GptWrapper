using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GptWrapper.Classes;

public class DllImportWrapper
{
    [DllImport("User32.dll")]
    public static extern bool SetForegroundWindow(nint hWnd);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern nint GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

    [DllImport("user32.dll")]
    public static extern int SendMessage(nint hWnd, int Msg, int wParam, int lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetWindowThreadProcessId(nint handle, out int processId);

    public static bool ApplicationIsActivated()
    {
        var activatedHandle = GetForegroundWindow();
        if (activatedHandle == nint.Zero)
        {
            return false;
        }

        var procId = Process.GetCurrentProcess().Id;
        int activeProcId;
        GetWindowThreadProcessId(activatedHandle, out activeProcId);

        return activeProcId == procId;
    }
}