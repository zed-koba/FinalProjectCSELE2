using LoginRegistration.ViewModel;
using Mopups.Pages;

namespace LoginRegistration.View;

public partial class AddNewComment : PopupPage
{
    public AddNewComment(PostCommentViewModel getChanges)
    {
        InitializeComponent();

        BindingContext = getChanges;
    }
}