#if WINDOWS
using Microsoft.UI.Windowing;
namespace utilities.Platforms.Windows;

public static class WindowChrome
{
    public static void BlockResize(Window mauiWindow)
    {
        var platformView = mauiWindow?.Handler?.PlatformView;
        if (platformView is null) return;

        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(platformView);
        var winId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
        var appWindow = AppWindow.GetFromWindowId(winId);
        if (appWindow.Presenter is OverlappedPresenter p)
        {
            p.IsResizable = false;
            p.IsMinimizable = false;
            p.IsMaximizable = false;
        }
    }
}
#endif