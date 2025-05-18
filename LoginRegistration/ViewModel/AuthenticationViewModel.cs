using LoginRegistration.Model;
using LoginRegistration.View;
using Mopups.Services;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows.Input;

namespace LoginRegistration.ViewModel
{
    public class AuthenticationViewModel
    {
        private readonly string apiAddress = "https://6821fa55b342dce8004c96f3.mockapi.io/UserDetails";
        private readonly HttpClient _client;
        private AuthenticationModel getUser { get; set; } = new();

        #region Commands

        public ICommand GetUserDetails { get; }
        public ICommand AddUser { get; }

        #endregion Commands

        private string _getUsername;
        private string _getPassword;
        private string _getFullName;
        private string _getEmailAdd;

        #region publicAttributes

        public string GetUsername
        {
            get => _getUsername;
            set
            {
                if (_getUsername != value)
                {
                    _getUsername = value;
                    OnPropertyChanged(GetUsername);
                }
            }
        }

        public string GetPassword
        {
            get => _getPassword;
            set
            {
                if (_getPassword != value)
                {
                    _getPassword = value;
                    OnPropertyChanged(GetPassword);
                }
            }
        }

        public string GetFullName
        {
            get => _getFullName;
            set
            {
                if (_getFullName != value)
                {
                    _getFullName = value;
                    OnPropertyChanged(_getFullName);
                }
            }
        }

        public string GetEmailAddress
        {
            get => _getEmailAdd;
            set
            {
                if (_getEmailAdd != value)
                {
                    _getEmailAdd = value;
                    OnPropertyChanged(_getEmailAdd);
                }
            }
        }

        #endregion publicAttributes

        public AuthenticationViewModel()
        {
            _client = new HttpClient();

            GetUserDetails = new Command(async () => await GetUserDetailsAsync());
            AddUser = new Command(async () => await AddUserAsync());
        }

        public async Task<bool> CheckUserExistsAsync()
        {
            try
            {
                var getAllDataResponse = await _client.GetFromJsonAsync<List<AuthenticationModel>>(apiAddress);
                var getUserId = getAllDataResponse?.FirstOrDefault(u => u.username.Equals(GetUsername, StringComparison.Ordinal));
                if (getUserId != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            return false;
        }

        private async Task GetUserDetailsAsync()
        {
            try
            {
                await MopupService.Instance.PushAsync(new LoginViewLoading());
                var getAllDataResponse = await _client.GetFromJsonAsync<List<AuthenticationModel>>(apiAddress);
                var getUserId = getAllDataResponse?.FirstOrDefault(u => u.username.Equals(GetUsername, StringComparison.Ordinal) && u.password.Equals(GetPassword, StringComparison.Ordinal));
                if (getUserId == null)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Invalid credentials, retry again", "OK");
                    await MopupService.Instance.PopAsync();
                    return;
                }
                var userId = $"{apiAddress}/{getUserId.id}";
                var response = await _client.GetStringAsync(userId);
                var deserialize = JsonSerializer.Deserialize<AuthenticationModel>(response);
                if (deserialize != null)
                {
                    getUser = deserialize;
                    App.Current.MainPage = new Homepage(getUser);
                    await MopupService.Instance.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task<bool> AddUserAsync()
        {
            try
            {
                var newUser = new AuthenticationModel
                {
                    dateCreated = ((DateTimeOffset)DateTime.Today).ToUnixTimeSeconds(),
                    avatar = "https://static.wikia.nocookie.net/fanon-brainrot/images/a/ac/Tralalero_tralala.jpg",
                    fullName = GetFullName,
                    username = GetUsername,
                    emailAddress = GetEmailAddress,
                    password = GetPassword,
                    totalComments = 0,
                    totalLikes = 0,
                    totalPosts = 0
                };

                var response = await _client.PostAsJsonAsync(apiAddress, newUser);
                if (response.IsSuccessStatusCode)
                {
                    await MopupService.Instance.PopAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            return false;
        }

        private event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}