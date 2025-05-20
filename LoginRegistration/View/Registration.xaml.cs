using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using LoginRegistration.ViewModel;
using Mopups.Services;
using System.Text.RegularExpressions;

namespace LoginRegistration.View;

public partial class Registration : ContentPage
{
    public Registration()
    {
        InitializeComponent();
        BindingContext = new AuthenticationViewModel();
    }

    private async void createAccount_Clicked(object sender, EventArgs e)
    {
        if (emptyErrorHandling(txtName) || emptyErrorHandling(txtUsername) ||
            emptyErrorHandling(txtPassword) || emptyErrorHandling(txtconfirmPass))
        {
            await Toast.Make("All fields are required", ToastDuration.Short, 12).Show();
            return;
        }

        if (txtPassword.Text.Length < 8)
        {
            await Toast.Make("Password must be at least 8 characters", ToastDuration.Short, 12).Show();
            return;
        }

        if (txtPassword.Text != txtconfirmPass.Text)
        {
            await Toast.Make("Passwords do not match", ToastDuration.Short, 12).Show();
            return;
        }

        if (!AgreeCheckBox.IsChecked)
        {
            await Toast.Make("Must agree to terms and condition", ToastDuration.Short, 12).Show();
            return;
        }

        if (!Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            await Toast.Make("Please enter a valid email address.", ToastDuration.Short, 12).Show();
            return;
        }
        await MopupService.Instance.PushAsync(new LoginViewLoading());

        if (BindingContext is AuthenticationViewModel vm)
        {
            if (await vm.CheckUserExistsAsync())
            {
                await Toast.Make("Username already exists", ToastDuration.Short, 12).Show();
                await MopupService.Instance.PopAsync();
                return;
            }
            App.Current.MainPage = new MainPage();
            vm.AddUser.Execute(null);
            await App.Current.MainPage.DisplayAlert("Announcement", "Successfully Registered", "OK");
            await MopupService.Instance.PopAsync();
        }

        // Clear form fields
        txtUsername.Text = "";
        txtName.Text = "";
        txtEmail.Text = "";
        txtPassword.Text = "";
        txtconfirmPass.Text = "";
        AgreeCheckBox.IsChecked = false;
    }

    private bool emptyErrorHandling(Entry entry)
    {
        if (string.IsNullOrEmpty(entry.Text))
        {
            return true;
        }
        return false;
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        App.Current.MainPage = new MainPage();
    }
}