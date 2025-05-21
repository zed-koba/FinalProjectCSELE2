using LoginRegistration.ViewModel;
namespace LoginRegistration
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new AuthenticationViewModel();
        }
        private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            App.Current.MainPage = new View.Registration();
        }
    }
}