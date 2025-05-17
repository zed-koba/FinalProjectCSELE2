using LoginRegistration.Model;
using LoginRegistration.ViewModel;
using Mopups.Pages;

namespace LoginRegistration.View;

public partial class PostComment : PopupPage
{
    public PostComment(ViewAllPostsModel getPostDetails, string userId)
    {
        InitializeComponent();
        var viewModel = new PostCommentViewModel(getPostDetails, userId);
        BindingContext = viewModel;

        _ = viewModel.GetCommentUserDetails();
    }
}