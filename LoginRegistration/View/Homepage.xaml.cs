using LoginRegistration.Model;
using LoginRegistration.ViewModel;

namespace LoginRegistration.View;

public partial class Homepage : ContentPage
{
    private bool _isRefreshing = false;
    public AuthenticationModel getUser;

    public Homepage(AuthenticationModel userGet)
    {
        InitializeComponent();
        getUser = userGet;
        var vm = new PostViewModel();
        vm.GetCurrentUserID = userGet.id;
        vm.CurrentUserDetail = userGet;
        BindingContext = vm;
        vm.refreshFeed.Execute(null);
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        App.Current.MainPage = new ProfilePage(getUser);
    }
}