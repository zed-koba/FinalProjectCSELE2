using CommunityToolkit.Maui;
using FFImageLoading.Maui;
using Microsoft.Extensions.Logging;
using Mopups.Hosting;

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
                .ConfigureMopups()
                .UseFFImageLoading()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Poppins-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("Poppins-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Poppins-Mediumn.ttf", "PoppinsMedium");
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