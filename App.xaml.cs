using Microsoft.Maui.Controls;

namespace utilities;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		//MainPage = new AppShell();
     /*Window secondWindow = new Window(new Page());
Application.Current?.OpenWindow(secondWindow);
*/
	}
  /*  protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
    */
    

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
