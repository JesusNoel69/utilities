namespace utilities;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
        var secondWindow = new Window(new ContentPage())
        {
            Title = "Second window",
            Width = 700,
            Height = 500,
            X = 100,
            Y = 100
        };

        Current?.OpenWindow(secondWindow);

	}
    
    

	 protected override void OnStart()
    {
        base.OnStart();
    }

    protected override void OnResume()
    {
        base.OnResume();
    }

    protected override void OnSleep()
    {
        base.OnSleep();
    }
}
