using Microsoft.Extensions.Logging;
using utilities.Interfaces;
using utilities.Platforms.Windows;
using utilities.Views;
using CommunityToolkit.Maui;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace utilities;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder.Services.AddSingleton<IWindowCreator, WindowCreator>();
#if WINDOWS
		builder.Services.AddSingleton<IScreenColorPicker, ScreenColorPicker>();
#endif
		builder.Services.AddTransient<ColorPickerPage>();
		builder.Services.AddTransient<FileManagerPage>();
		builder.Services.AddSingleton<AppShell>();
		builder
			.UseMauiApp<App>()
			.UseSkiaSharp()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
