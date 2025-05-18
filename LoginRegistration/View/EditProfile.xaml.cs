using LoginRegistration.Model;
using LoginRegistration.ViewModel;
using Mopups.Pages;

namespace LoginRegistration.View;

public partial class EditProfile : PopupPage
{
    public EditProfile(AuthenticationModel userDetails)
    {
        InitializeComponent();
        BindingContext = new ProfileViewModel(userDetails);
    }
}