using LoginRegistration.ViewModel;
using Mopups.Pages;

namespace LoginRegistration.View;

public partial class Post : PopupPage
{
    public Post(PostViewModel viewModel)
    {
        InitializeComponent();
        var vm = viewModel;
        BindingContext = vm;
        vm.refreshFeed.Execute(null);
    }
}