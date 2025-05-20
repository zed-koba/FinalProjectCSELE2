using CommunityToolkit.Maui.Alerts;
using LoginRegistration.Model;
using LoginRegistration.View;
using Mopups.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Json;
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
        private string _newAvatar;

        #endregion private Attributes

        #region public Attributes

        public ObservableCollection<UserInteractionModel> UserPosts { get; set; } = new();

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

        public string NewAvatar
        {
            get => _newAvatar;
            set
            {
                if (_newAvatar != value)
                {
                    _newAvatar = value;
                    OnPropertyChanged(nameof(NewAvatar));
                }
            }
        }

        public ICommand showEditProfile { get; }
        public ICommand updateProfile { get; }
        public ICommand showChangeAvatar { get; }
        public ICommand cancelEditProfile { get; }
        public ICommand refreshProfile { get; }
        public ICommand showTappedPost { get; }
        public ICommand profileOptions { get; }
        public ICommand showProfileOptions { get; }

        #endregion public Attributes

        public ProfileViewModel(AuthenticationModel getUserDetails)
        {
            _httpClient = new HttpClient();
            UserDetails = getUserDetails;

            _newFullName = UserDetails.fullName;
            _newUsername = UserDetails.username;
            _newPassword = UserDetails.password;
            _newAvatar = UserDetails.avatar;

            showEditProfile = new Command(async () => await showEditProfileAsync());
            updateProfile = new Command(async () => await updateProfileAsync());
            showChangeAvatar = new Command(async () => await showChangeAvatarAsync());
            cancelEditProfile = new Command(async () => await cancelEditProfileAsync());
            refreshProfile = new Command(async () => await refreshProfileAsync());
            showTappedPost = new Command<UserInteractionModel>(OnImageTapped);
            showProfileOptions = new Command(async () => await MopupService.Instance.PushAsync(new ProfileOptionsPopup(this)));
            profileOptions = new Command<string>(OnProfileOptionsPressed);
        }

        private async void OnProfileOptionsPressed(string parameter)
        {
            if (parameter == "editProfile")
            {
                await showEditProfileAsync();
            }
            else if (parameter == "logout")
            {
                await MopupService.Instance.PopAsync();
                App.Current.MainPage = new MainPage();
            }
        }

        private async void OnImageTapped(UserInteractionModel postDetail)
        {
            var checkIfCurrentUserLikePost = postDetail.Posts.like.IndexOf(UserDetails.id);
            var isLiked = false;
            if (checkIfCurrentUserLikePost == -1)
            {
                isLiked = false;
            }
            else
            {
                isLiked = true;
            }

            string likeColor = isLiked ? "#FB3137" : "White";
            string iconFont = isLiked ? "FAsolid" : "FAregular";
            var setViewAllPostModel = new ViewAllPostsModel
            {
                Posts = postDetail.Posts,
                UserDetailId = postDetail.UserDetailId,
                postId = postDetail.postId,
                avatar = UserDetails.avatar,
                fullName = UserDetails.fullName,
                likeColor = likeColor,
                iconFont = iconFont
            };
            var tcs = new TaskCompletionSource<bool>();
            var popup = new PostComment(setViewAllPostModel, UserDetails, tcs);
            await MopupService.Instance.PushAsync(popup);
            var result = await tcs.Task;
            if (result)
            {
                await refreshProfileAsync();
            }
        }

        private async Task refreshProfileAsync()
        {
            try
            {
                var getUserPostsUrl = $"{ApiUrl}/UserDetails/{UserDetails.id}/UserInteractions";
                var getUserPosts = await _httpClient.GetFromJsonAsync<List<UserInteractionModel>>(getUserPostsUrl);
                if (getUserPosts != null)
                {
                    UserPosts.Clear();
                    foreach (var item in getUserPosts)
                    {
                        UserPosts.Add(item);
                    }
                }

                var getUserDetailsUrl = $"{ApiUrl}/UserDetails/{UserDetails.id}";
                var getUserDetails = await _httpClient.GetFromJsonAsync<AuthenticationModel>(getUserDetailsUrl);
                if (getUserDetails != null)
                {
                    NewUsername = getUserDetails?.username!;
                    NewAvatar = getUserDetails?.avatar!;
                    NewFullName = getUserDetails?.fullName!;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private async Task cancelEditProfileAsync()
        {
            NewFullName = UserDetails.fullName;
            NewUsername = UserDetails.username;
            NewPassword = UserDetails.password;
            NewAvatar = UserDetails.avatar;

            await MopupService.Instance.PopAsync();
            await MopupService.Instance.PopAsync();
        }

        private async Task showChangeAvatarAsync()
        {
            await MopupService.Instance.PushAsync(new EditAvatar(this));
        }

        private async Task updateProfileAsync()
        {
            try
            {
                var getUserUrl = $"{ApiUrl}/UserDetails/{UserDetails.id}";
                if (NewFullName != null && NewPassword != null && NewPassword != null)
                {
                    UserDetails.fullName = NewFullName;
                    UserDetails.username = NewUsername;
                    UserDetails.password = NewPassword;
                    UserDetails.avatar = NewAvatar;

                    var updateUserDetails = await _httpClient.PutAsJsonAsync(getUserUrl, UserDetails);
                    if (updateUserDetails.IsSuccessStatusCode)
                    {
                        await MopupService.Instance.PopAsync();
                        await MopupService.Instance.PopAsync();
                    }
                }
                else
                {
                    await Toast.Make("All fields are required").Show();
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("ERROR", ex.Message, "OK");
            }
        }

        private async Task showEditProfileAsync()
        {
            await MopupService.Instance.PushAsync(new EditProfile(this));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}