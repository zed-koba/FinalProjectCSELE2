using LoginRegistration.ViewModel;
using Mopups.Pages;
using Mopups.Services;

namespace LoginRegistration.View;

public partial class EditAvatar : PopupPage
{
    private string source = string.Empty;
    private ProfileViewModel profileViewModel;

    public EditAvatar(ProfileViewModel vm)
    {
        InitializeComponent();
        profileViewModel = vm;
        BindingContext = profileViewModel;

        foreach (var child in flexLayout.Children)
        {
            if (child is Border border && border.Content is Image img && img.Source is UriImageSource uri)
            {
                if (uri.Uri.ToString() == vm.NewAvatar)
                {
                    border.Stroke = Color.FromArgb("FB3137");
                }
            }
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        MopupService.Instance.PopAsync();
    }

    private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        if (sender is Border tappedBorder)
        {
            foreach (var child in flexLayout.Children)
            {
                if (child is Border border)
                {
                    border.Stroke = Color.FromArgb("#888");
                }
            }
            tappedBorder.Stroke = Color.FromArgb("#FB3137");
            if (tappedBorder.Content is Image image && image.Source is UriImageSource uri)
            {
                source = uri.Uri.ToString()!;
                profileViewModel.NewAvatar = source;
            }
        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        profileViewModel.NewAvatar = source;
        MopupService.Instance.PopAsync();
    }
}