using LoginRegistration.ViewModel;
using Mopups.Pages;

namespace LoginRegistration.View;

public partial class EditProfile : PopupPage
{
    public EditProfile(ProfileViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}