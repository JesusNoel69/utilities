namespace utilities
{
    public class WindowCreator : IWindowCreator
{
    public Window CreateWindow(Application app, IActivationState? activationState)
    {
        // Get display size
       var di = DeviceDisplay.Current.MainDisplayInfo;
        var screenW = di.Width / di.Density;
        var screenH = di.Height / di.Density;
        var taskbarDip = 40;
        var width = screenW / 4.0;
        var height = screenH - taskbarDip;
        var window = new Window(new AppShell())
        {
            Width = width,
            Height = height,
            X = screenW - width + 8,
            Y = 0,
        };
  window.HandlerChanged += (_, __) =>
        {
#if WINDOWS
            Platforms.Windows.WindowChrome.BlockResize(window);
#endif
        };
        return window;
    }
}
}