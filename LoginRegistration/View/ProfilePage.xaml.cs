using LoginRegistration.Model;
using LoginRegistration.ViewModel;

namespace LoginRegistration.View;

public partial class ProfilePage : ContentPage
{
    public ProfilePage(AuthenticationModel getUserDetails)
    {
        InitializeComponent();
        BindingContext = new ProfileViewModel(getUserDetails);
    }
}