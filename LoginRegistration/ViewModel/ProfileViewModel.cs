using LoginRegistration.Model;
using LoginRegistration.View;
using Mopups.Services;
using System.ComponentModel;
using System.Windows.Input;

namespace LoginRegistration.ViewModel
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private readonly HttpClient _httpClient;
        private readonly string ApiUrl = "https://6821fa55b342dce8004c96f3.mockapi.io";

        #region private Attributes

        private AuthenticationModel _userDetails { get; set; } = new();
        private string _newFullName;
        private string _newUsername;
        private string _newPassword;

        #endregion private Attributes

        #region public Attributes

        public AuthenticationModel UserDetails
        {
            get => _userDetails;
            set
            {
                _userDetails = value;
                OnPropertyChanged(nameof(UserDetails));
            }
        }

        public string NewFullName
        {
            get => _newFullName;
            set
            {
                if (_newFullName != value)
                {
                    _newFullName = value;
                    OnPropertyChanged(nameof(NewFullName));
                }
            }
        }

        public string NewUsername
        {
            get => _newUsername;
            set
            {
                if (_newUsername != value)
                {
                    _newUsername = value;
                    OnPropertyChanged(nameof(NewUsername));
                }
            }
        }

        public string NewPassword
        {
            get => _newPassword;
            set
            {
                if (_newPassword != value)
                {
                    _newPassword = value;
                    OnPropertyChanged(nameof(NewPassword));
                }
            }
        }

        public ICommand showEditProfile { get; }

        #endregion public Attributes

        public ProfileViewModel(AuthenticationModel getUserDetails)
        {
            _httpClient = new HttpClient();
            UserDetails = getUserDetails;

            showEditProfile = new Command(async () => await showEditProfileAsync());
        }

        private async Task showEditProfileAsync()
        {
            await MopupService.Instance.PushAsync(new EditProfile(UserDetails));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}