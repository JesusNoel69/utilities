namespace utilities
{
    public class WindowCreator : IWindowCreator
{
    public Window CreateWindow(Application app, IActivationState activationState)
    {
        /*var window = new Window(new ContentPage
        {
            Content = new Grid
            {
                new Label
                {
                    Text = "Hello from IWindowCreator",
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                }
            }
        });*/
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
            X = screenW - width+8,
            Y = 0,
        };
        var second = new Window(new ContentPage())
        {
            Title = "Second",
            Width = 700,
            Height = 500,
            X = 950,
            Y = 0
        };

        //app.OpenWindow(second);
        return window;
    }
}
}