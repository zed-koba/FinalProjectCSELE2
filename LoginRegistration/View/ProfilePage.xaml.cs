using LoginRegistration.Model;
using LoginRegistration.ViewModel;
using Mopups.Services;

namespace LoginRegistration.View;

public partial class ProfilePage : ContentPage
{
    private AuthenticationModel UserDetails;

    public ProfilePage(AuthenticationModel getUserDetails)
    {
        InitializeComponent();
        UserDetails = getUserDetails;
        var vm = new ProfileViewModel(getUserDetails);
        BindingContext = vm;
        vm.refreshProfile.Execute(null);
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        App.Current.MainPage = new Homepage(UserDetails);
        await MopupService.Instance.PushAsync(new LoginViewLoading());
    }
}