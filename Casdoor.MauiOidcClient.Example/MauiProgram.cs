using Microsoft.Extensions.Logging;

namespace Casdoor.MauiOidcClient.Example
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
		    builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton(new CasdoorClient(new()
            {
                Domain = "exlens.ru",
                ClientId = "b31be2ee91ee49961506",
                Scope = "openid profile email",

#if WINDOWS
			RedirectUri = "http://localhost/callback"
#else
                RedirectUri = "casdoor://callback"
#endif
            }));
            builder.Services.AddSingleton<MainPage>();
            return builder.Build();
        }
    }
}