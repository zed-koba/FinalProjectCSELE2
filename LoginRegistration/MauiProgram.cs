using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace LoginRegistration
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Poppins-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("Poppins-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("fontello.ttf", "FontelloFamily");
                    fonts.AddFont("fa-regular.ttf", "FAregular");
                    fonts.AddFont("fa-solid.ttf", "FAsolid");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
