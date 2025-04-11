using Microsoft.Extensions.Logging;
using Microsoft.Maui.Hosting;
using MyMauiAppAndroid.Platforms.Android;

namespace MyMauiAppAndroid;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<VideoPlayerView, VideoPlayerHandler>();
            });
			// .ConfigureFonts(fonts =>
			// {
			// 	fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			// 	fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			// });

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
