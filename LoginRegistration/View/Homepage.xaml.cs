using LoginRegistration.Model;
using LoginRegistration.ViewModel;
using Mopups.Services;

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
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is PostViewModel vm)
        {
            await vm.refreshFeedAsync();
            if (MopupService.Instance.PopupStack.Any())
                await MopupService.Instance.PopAsync();
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        App.Current.MainPage = new ProfilePage(getUser);
    }
}