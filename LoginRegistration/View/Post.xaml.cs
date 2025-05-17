using LoginRegistration.ViewModel;
using Mopups.Pages;

namespace LoginRegistration.View;

public partial class Post : PopupPage
{
    public Post()
    {
        InitializeComponent();
        BindingContext = new PostViewModel();
    }
}