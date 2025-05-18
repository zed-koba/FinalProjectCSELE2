using LoginRegistration.ViewModel;
using Mopups.Pages;

namespace LoginRegistration.View;

public partial class EditPost : PopupPage
{
    public EditPost(PostCommentViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}