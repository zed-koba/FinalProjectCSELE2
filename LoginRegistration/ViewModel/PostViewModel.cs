using LoginRegistration.Model;
using LoginRegistration.View;
using Mopups.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows.Input;

namespace LoginRegistration.ViewModel
{
    public class PostViewModel : INotifyPropertyChanged
    {
        private readonly HttpClient _httpClient;
        private readonly string apiUrl = "https://6821fa55b342dce8004c96f3.mockapi.io";
        private readonly string clientID = "30c52e92e523c99";

        #region privateDeclarations

        private string? _getCaptions;
        private string? _getImageLink;
        private FileResult? result;
        private string _getImageName = "File Name";
        private bool isRefreshing;

        #endregion privateDeclarations

        #region publicDeclarations

        public ObservableCollection<ViewAllPostsModel> allPosts { get; set; } = new();

        public string GetCurrentUserID { get; set; }
        public AuthenticationModel CurrentUserDetail { get; set; }

        public string GetCaption
        {
            get => _getCaptions;
            set
            {
                if (_getCaptions != value)
                {
                    _getCaptions = value;
                    OnPropertyChanged(nameof(GetCaption));
                }
            }
        }

        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;
                    OnPropertyChanged(nameof(IsRefreshing));
                }
            }
        }

        public string GetImageName
        {
            get => _getImageName;
            set
            {
                if (_getImageName != value)
                {
                    _getImageName = value;
                    OnPropertyChanged(nameof(GetImageName));
                }
            }
        }

        public ICommand addPost { get; }
        public ICommand showNewPost { get; }
        public ICommand closeNewPost { get; }
        public ICommand uploadAPhoto { get; }
        public ICommand refreshFeed { get; }

        public ICommand showTappedPost { get; }
        public ICommand likeAPost { get; }

        #endregion publicDeclarations

        public PostViewModel()
        {
            _httpClient = new HttpClient();

            addPost = new Command(async () => await AddPost());
            showNewPost = new Command(async () => await showNewPostAsync());
            closeNewPost = new Command(async () => await closeNewPostAsync());
            uploadAPhoto = new Command(async () => await uploadAPhotoAsync());
            refreshFeed = new Command(async () => await refreshFeedAsync());
            showTappedPost = new Command<ViewAllPostsModel>(OnImageTapped);
            likeAPost = new Command<ViewAllPostsModel>(OnLikeIconTapped);
        }

        private async void OnImageTapped(ViewAllPostsModel postDetail)
        {
            await MopupService.Instance.PushAsync(new PostComment(postDetail, GetCurrentUserID));
        }

        private async void OnLikeIconTapped(ViewAllPostsModel postDetail)
        {
            try
            {
                var GetPostDetailsUrl = $"{apiUrl}/UserDetails/{postDetail.UserDetailId}/UserInteractions/{postDetail.postId}";
                var GetUserDetails = $"{apiUrl}/UserDetails/{GetCurrentUserID}";
                var newData = new UserInteractionModel()
                {
                    Posts = postDetail.Posts,
                    UserDetailId = postDetail.UserDetailId,
                    postId = postDetail.postId,
                };
                var checkIfLiked = postDetail.Posts.like.IndexOf(GetCurrentUserID);
                bool ifLiked = false;
                if (checkIfLiked == -1)
                {
                    newData.Posts.like.Add($"{GetCurrentUserID}");
                    CurrentUserDetail.totalLikes = CurrentUserDetail.totalLikes + 1;
                    ifLiked = false;
                }
                else
                {
                    newData.Posts.like.RemoveAt(checkIfLiked);
                    CurrentUserDetail.totalLikes = CurrentUserDetail.totalLikes - 1;
                    ifLiked = true;
                }

                var updatePostLike = await _httpClient.PutAsJsonAsync(GetPostDetailsUrl, newData);
                var updateTotalLikes = await _httpClient.PutAsJsonAsync(GetUserDetails, CurrentUserDetail);
                var targetPost = allPosts.FirstOrDefault(u => u.postId == postDetail.postId);
                if (targetPost != null)
                {
                    targetPost.Posts.like = newData.Posts.like;
                    targetPost.likeColor = ifLiked ? "White" : "#FB3137";
                    targetPost.iconFont = ifLiked ? "FAregular" : "FAsolid";
                    var index = allPosts.IndexOf(targetPost);
                    if (index != -1)
                    {
                        allPosts.RemoveAt(index);
                        allPosts.Insert(index, targetPost);
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public async Task refreshFeedAsync()
        {
            string getAllPostsUrl = $"{apiUrl}/UserInteractions";
            string getUserInfoUrl = $"{apiUrl}/UserDetails";
            var getPosts = await _httpClient.GetFromJsonAsync<List<ViewAllPostsModel>>(getAllPostsUrl);
            var getUserInfo = await _httpClient.GetFromJsonAsync<List<AuthenticationModel>>(getUserInfoUrl);

            if (getPosts != null && getUserInfo != null)
            {
                allPosts.Clear();
                var sortedPosts = getPosts.OrderByDescending(post => post.Posts.timeStamp);

                foreach (ViewAllPostsModel post in sortedPosts)
                {
                    var user = getUserInfo.FirstOrDefault(x => x.id == post.UserDetailId);
                    post.avatar = user?.avatar!;
                    post.fullName = user?.fullName!;
                    bool isLiked = post.Posts.like.Any(like => like != null &&
                        like.Contains(GetCurrentUserID));

                    post.likeColor = isLiked ? "#FB3137" : "White";
                    post.iconFont = isLiked ? "FAsolid" : "FAregular";
                    allPosts.Add(post);
                }
            }
            await Task.Delay(2000);
            IsRefreshing = false;
        }

        private async Task uploadAPhotoAsync()
        {
            //Pick a file to upload an image
            try
            {
                result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Pick an Image",
                    FileTypes = FilePickerFileType.Images
                });

                if (result == null)
                    return;

                if (result != null)
                    GetImageName = result.FileName;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task showNewPostAsync()
        {
            await MopupService.Instance.PushAsync(new Post(GetCurrentUserID), true);
        }

        private async Task closeNewPostAsync()
        {
            await MopupService.Instance.PopAsync();
        }

        private async Task AddPost()
        {
            await MopupService.Instance.PushAsync(new LoadingScreen());
            try
            {
                //Upload the picked file to Imgur API
                using var stream = await result!.OpenReadAsync();
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                var imageBytes = ms.ToArray();
                string base64Image = Convert.ToBase64String(imageBytes);

                using var _uploadClient = new HttpClient();
                _uploadClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", clientID);

                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"image", base64Image},
                    {"type", "base64" }
                });

                var response = await _uploadClient.PostAsync("https://api.imgur.com/3/image", content);
                response.EnsureSuccessStatusCode();

                //getting the upload Image link
                var jsonString = await response.Content.ReadAsStringAsync();
                var json = JsonDocument.Parse(jsonString);
                _getImageLink = json.RootElement.GetProperty("data").GetProperty("link").GetString();

                //POST / Add the data to the MockAPI

                var newPost = new UserInteractionModel()
                {
                    Posts = new PostModel()
                    {
                        imageSource = _getImageLink!,
                        caption = GetCaption,
                        timeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                        comments = new ObservableCollection<CommentModel>(),
                        like = new ObservableCollection<string>()
                    },
                    UserDetailId = GetCurrentUserID
                };
                var getUserUrl = $"{apiUrl}/UserInteractions";
                var addNewPost = await _httpClient.PostAsJsonAsync(getUserUrl, newPost);
                if (addNewPost.IsSuccessStatusCode)
                {
                    await closeNewPostAsync();
                    await closeNewPostAsync();
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}