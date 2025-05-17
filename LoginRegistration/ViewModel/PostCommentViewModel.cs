using LoginRegistration.Model;
using LoginRegistration.View;
using Mopups.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Windows.Input;

namespace LoginRegistration.ViewModel
{
    public class PostCommentViewModel : INotifyPropertyChanged
    {
        private readonly HttpClient _httpClient;
        private readonly string apiUrl = "https://6821fa55b342dce8004c96f3.mockapi.io";

        #region privateAttributes

        private string currentUserId { get; set; } = string.Empty;
        private ViewAllPostsModel postDetails = new();
        public ObservableCollection<ViewAllCommentsModel> commentDetails { get; set; } = new();
        private ObservableCollection<CommentModel> comments { get; set; } = new();
        private string _getPostComment;

        #endregion privateAttributes

        #region publicAttributes

        public string GetPostComment
        {
            get => _getPostComment;
            set
            {
                if (_getPostComment != value)
                {
                    _getPostComment = value;
                    OnPropertyChanged(nameof(GetPostComment));
                }
            }
        }

        public ViewAllPostsModel PostDetails
        {
            get => postDetails;
            set
            {
                postDetails = value;
                OnPropertyChanged(nameof(postDetails));
            }
        }

        public ICommand GetPostData { get; }
        public ICommand showWriteComment { get; }
        public ICommand updatePostComments { get; }

        #endregion publicAttributes

        public PostCommentViewModel(ViewAllPostsModel getPostDetails, string userId)
        {
            _httpClient = new HttpClient();

            postDetails = getPostDetails;
            currentUserId = userId;

            GetPostData = new Command(async () => await GetPostDataAsync());
            showWriteComment = new Command(async () => await showWriteCommentAsync());
            updatePostComments = new Command(async () => await updatePostCommentsAsync());
        }

        public async Task GetCommentUserDetails()
        {
            string getUserInfoUrl = $"{apiUrl}/UserDetails";
            var getUserInfo = await _httpClient.GetFromJsonAsync<List<AuthenticationModel>>(getUserInfoUrl);

            if (getUserInfo != null)
            {
                var sortedPosts = postDetails.Posts.comments.OrderByDescending(c => c.commentTimeStamp);
                foreach (CommentModel com in sortedPosts)
                {
                    var user = getUserInfo.FirstOrDefault(x => x.id == com.userId);
                    commentDetails.Add(new ViewAllCommentsModel
                    {
                        userId = com.userId,
                        commentId = com.commentId,
                        commentPost = com.commentPost,
                        commentTimeStamp = com.commentTimeStamp,
                        avatar = user?.avatar!,
                        fullName = user?.fullName!
                    });
                    comments.Add(com);
                }
            }
        }

        private async Task updatePostCommentsAsync()
        {
            try
            {
                string uniqueCommentId = $"comms_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}_{Random.Shared.Next(1000, 9999)}";
                var newComment = new CommentModel
                {
                    userId = currentUserId,
                    commentId = uniqueCommentId,
                    commentPost = GetPostComment,
                    commentTimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                };
                comments.Add(newComment);
                postDetails.Posts.comments = comments;
                var getPostUrl = $"{apiUrl}/UserDetails/{postDetails.UserDetailId}/UserInteractions/{postDetails.postId}";
                var updatePostData = await _httpClient.PutAsJsonAsync(getPostUrl, postDetails);
                if (updatePostData.IsSuccessStatusCode)
                {
                    commentDetails.Add(new ViewAllCommentsModel
                    {
                        userId = newComment.userId,
                        commentId = newComment.commentId,
                        commentPost = newComment.commentPost,
                        commentTimeStamp = newComment.commentTimeStamp,
                        avatar = postDetails.avatar,
                        fullName = postDetails.fullName
                    });
                    await MopupService.Instance.PopAsync();
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task showWriteCommentAsync()
        {
            await MopupService.Instance.PushAsync(new AddNewComment(this), true);
        }

        private async Task GetPostDataAsync()
        {
            try
            {
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