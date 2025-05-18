using LoginRegistration.ViewModel;
using Mopups.Pages;

namespace LoginRegistration.View;

public partial class OptionsPopup : PopupPage
{
    public OptionsPopup(PostCommentViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}