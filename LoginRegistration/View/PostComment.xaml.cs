using LoginRegistration.Model;
using LoginRegistration.ViewModel;
using Mopups.Pages;

namespace LoginRegistration.View;

public partial class PostComment : PopupPage
{
    public PostComment(ViewAllPostsModel getPostDetails, AuthenticationModel GetUserDetails, TaskCompletionSource<bool> tcs)
    {
        InitializeComponent();
        var viewModel = new PostCommentViewModel(getPostDetails, GetUserDetails);
        viewModel._tcs = tcs;
        BindingContext = viewModel;
        _ = viewModel.GetCommentUserDetails();
    }
}