using LoginRegistration.View;
using Microsoft.Maui.Platform;

namespace LoginRegistration
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new ProfilePage();
#if ANDROID
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
            {
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
            });
#endif
        }

    }
}
