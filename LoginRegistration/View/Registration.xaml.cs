
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Alerts;
using System.Xml.Linq;

namespace LoginRegistration.View;

public partial class Registration : ContentPage
{
    string filePath = Path.Combine(FileSystem.AppDataDirectory, "accounts.txt");
    public Registration()
    {
        InitializeComponent();
    }

    private async void createAccount_Clicked(object sender, EventArgs e)
    {
        PrintUserFile();
        if (emptyErrorHandling(txtName) || emptyErrorHandling(txtUsername) || emptyErrorHandling(txtPassword) || emptyErrorHandling(txtconfirmPass))
        {
            var toast = Toast.Make("All fields are required", ToastDuration.Short, 12);
            await toast.Show();
        }
        else
        {
            if (txtPassword.Text.Length < 8)
            {
                await Toast.Make("Password must be atleast 8 characters", ToastDuration.Short, 12).Show();
            }
            else
            {
                if (txtPassword.Text != txtconfirmPass.Text)
                {
                    await Toast.Make("Passwords do not match", ToastDuration.Short, 12).Show();
                }
                else
                {
                    if (!AgreeCheckBox.IsChecked)
                    {
                        await Toast.Make("Must agree to terms and condition", ToastDuration.Short, 12).Show();
                    }
                    else
                    {
                        if (File.ReadLines(filePath).Any(line => line.Split(',')[1] == txtUsername.Text))
                        {
                            await Toast.Make("Username already exists", ToastDuration.Short, 12).Show();
                            return;
                        }

                        File.AppendAllText(filePath, $"{txtName.Text},{txtUsername.Text},{txtPassword.Text}\n");
                        await Toast.Make("Successfully Registered", ToastDuration.Short, 12).Show();
                        txtUsername.Text = "";
                        txtName.Text = "";
                        txtPassword.Text = "";
                        txtconfirmPass.Text = "";
                        AgreeCheckBox.IsChecked = false;
                    }
                }
            }
        }
    }

    private bool emptyErrorHandling(Entry entry)
    {
        if (string.IsNullOrEmpty(entry.Text))
        {
            return true;
        }
        return false;
    }
    private async void PrintUserFile()
    {
        if (File.Exists(filePath))
        {
            await Toast.Make("File Found", ToastDuration.Short, 12).Show();
        }
        else
        {
            File.WriteAllText(filePath, "");
            await Toast.Make("File Not Found. Sucessfully created new file", ToastDuration.Short, 12).Show();
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        App.Current.MainPage = new MainPage();
    }
}