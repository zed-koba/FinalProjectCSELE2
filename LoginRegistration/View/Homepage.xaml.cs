using LoginRegistration.ViewModel;

namespace LoginRegistration.View;

public partial class Homepage : ContentPage
{
    public Homepage()
    {
        InitializeComponent();
        BindingContext = new PostViewModel();
        if (BindingContext is PostViewModel vm)
        {
            vm.refreshFeed.Execute(null);
        }
    }
}