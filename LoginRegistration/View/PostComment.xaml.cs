using LoginRegistration.Model;
using LoginRegistration.ViewModel;
using Mopups.Pages;

namespace LoginRegistration.View;

public partial class PostComment : PopupPage
{
    public PostComment(ViewAllPostsModel getPostDetails, AuthenticationModel GetUserDetails)
    {
        InitializeComponent();
        var viewModel = new PostCommentViewModel(getPostDetails, GetUserDetails);
        BindingContext = viewModel;

        _ = viewModel.GetCommentUserDetails();
    }
}