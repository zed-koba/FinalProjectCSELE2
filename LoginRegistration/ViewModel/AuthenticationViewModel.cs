using LoginRegistration.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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
        #endregion
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
                if(_getFullName != value)
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
                if(_getEmailAdd != value)
                {
                    _getEmailAdd = value;
                    OnPropertyChanged(_getEmailAdd);
                }
            }
        }
        #endregion
        public AuthenticationViewModel()
        { 
            _client = new HttpClient();

            GetUserDetails = new Command(async ()=> await GetUserDetailsAsync());
            AddUser = new Command(async ()=> await AddUserAsync());
        }

        private async Task GetUserDetailsAsync()
        {
            try
            {
                var getAllDataResponse = await _client.GetFromJsonAsync<List<AuthenticationModel>>(apiAddress);
                var getUserId = getAllDataResponse?.FirstOrDefault(u => u.username.Equals(GetUsername, StringComparison.Ordinal) && u.password.Equals(GetPassword, StringComparison.Ordinal));
                if (getUserId == null)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Invalid credentials, retry again", "OK");
                    return;
                }
                var userId = $"{apiAddress}/{getUserId.Id}";
                var response = await _client.GetStringAsync(userId);
                var deserialize = JsonSerializer.Deserialize<AuthenticationModel>(response);
                if (deserialize != null)
                {
                    getUser = deserialize;
                    await App.Current.MainPage.DisplayAlert("Success", $"{getUser.fullName}", "OK");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task AddUserAsync()
        {
            var newUser = new AuthenticationModel
            {
                dateCreated = ((DateTimeOffset)DateTime.Today).ToUnixTimeSeconds(),
                avatar = "https://static.wikia.nocookie.net/fanon-brainrot/images/a/ac/Tralalero_tralala.jpg",

            };
        }

        private event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
