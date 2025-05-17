using LoginRegistration.ViewModel;
using Mopups.Pages;

namespace LoginRegistration.View;

public partial class Post : PopupPage
{
    public Post(string getUserId)
    {
        InitializeComponent();
        var vm = new PostViewModel();
        vm.GetCurrentUserID = getUserId;
        BindingContext = vm;
        vm.refreshFeed.Execute(null);
    }
}