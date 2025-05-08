using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using LoginRegistration;
namespace LoginRegistration
{
    public partial class MainPage : ContentPage
    {
        string filePath = Path.Combine(FileSystem.AppDataDirectory, "accounts.txt");

        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtUsername.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                await Toast.Make("All fields cannot be blank", ToastDuration.Short).Show();
                return;
            }

            if (checkAccount(txtUsername.Text, txtPassword.Text))
            {
                var getName = File.ReadLines(filePath)
                    .Select(line => line.Split(','))
                    .Where(parts => parts.Length > 1 && parts[1] == txtUsername.Text)
                    .FirstOrDefault();

                await Toast.Make("Successfully Logged In, " + getName[0], ToastDuration.Short).Show();
            }
            else
            {
                await Toast.Make("Invalid username or password", ToastDuration.Short).Show();

            }
        }

        private bool checkAccount(string username, string password)
        {
            return File.ReadLines(filePath).Any(line => line.Split(',')[1] == txtUsername.Text && line.Split(',')[2] == txtPassword.Text);
        }

        private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            App.Current.MainPage = new View.Registration();
        }
    }

}
