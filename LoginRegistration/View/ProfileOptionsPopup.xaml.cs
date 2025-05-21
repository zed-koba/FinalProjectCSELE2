using LoginRegistration.ViewModel;
using Mopups.Pages;

namespace LoginRegistration.View;

public partial class ProfileOptionsPopup : PopupPage
{
    public ProfileOptionsPopup(ProfileViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}