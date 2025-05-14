using Microsoft.Maui.Platform;
using LoginRegistration.View;

namespace LoginRegistration
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new Homepage();
#if ANDROID
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
            {
                handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
            });
#endif
        }

    }
}
