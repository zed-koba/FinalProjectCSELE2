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

        private ViewAllPostsModel postDetails = new();
        public ObservableCollection<ViewAllCommentsModel> commentDetails { get; set; } = new();
        private ObservableCollection<CommentModel> comments { get; set; } = new();
        private AuthenticationModel userDetail = new();
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
                OnPropertyChanged(nameof(PostDetails));
            }
        }

        public ICommand GetPostData { get; }
        public ICommand showWriteComment { get; }
        public ICommand updatePostComments { get; }
        public ICommand closeCommentPost { get; }
        public ICommand likeAPost { get; }

        #endregion publicAttributes

        public PostCommentViewModel(ViewAllPostsModel getPostDetails, AuthenticationModel GetUserDetails)
        {
            _httpClient = new HttpClient();

            postDetails = getPostDetails;
            userDetail = GetUserDetails;

            GetPostData = new Command(async () => await GetPostDataAsync());
            showWriteComment = new Command(async () => await showWriteCommentAsync());
            updatePostComments = new Command(async () => await updatePostCommentsAsync());
            closeCommentPost = new Command(async () => await MopupService.Instance.PopAsync());
            likeAPost = new Command(OnLikeIconTapped);
        }

        private async void OnLikeIconTapped()
        {
            try
            {
                var GetPostDetailsUrl = $"{apiUrl}/UserDetails/{PostDetails.UserDetailId}/UserInteractions/{PostDetails.postId}";
                var newData = new UserInteractionModel()
                {
                    Posts = PostDetails.Posts,
                    UserDetailId = PostDetails.UserDetailId,
                    postId = PostDetails.postId,
                };
                var checkIfLiked = PostDetails.Posts.like.IndexOf(userDetail.id);
                if (checkIfLiked == -1)
                {
                    newData.Posts.like.Add($"{userDetail.id}");
                    PostDetails.likeColor = "#FB3137";
                    PostDetails.iconFont = "FAsolid";
                    PostDetails.Posts.like = newData.Posts.like;
                }
                else
                {
                    newData.Posts.like.RemoveAt(checkIfLiked);
                    PostDetails.likeColor = "White";
                    PostDetails.iconFont = "FAregular";
                    PostDetails.Posts.like = newData.Posts.like;
                }
                var updatePostLike = await _httpClient.PutAsJsonAsync(GetPostDetailsUrl, newData);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
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
                    userId = userDetail.id,
                    commentId = uniqueCommentId,
                    commentPost = GetPostComment,
                    commentTimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                };
                comments.Add(newComment);
                postDetails.Posts.comments = comments;
                userDetail.totalComments = userDetail.totalComments + 1;
                var newUpdatePosts = new UserInteractionModel
                {
                    Posts = postDetails.Posts,
                    UserDetailId = postDetails.UserDetailId,
                    postId = postDetails.postId
                };
                var getPostUrl = $"{apiUrl}/UserDetails/{postDetails.UserDetailId}/UserInteractions/{postDetails.postId}";
                var userUrl = $"{apiUrl}/UserDetails/{userDetail.id}";
                var updatePostData = await _httpClient.PutAsJsonAsync(getPostUrl, newUpdatePosts);
                var updateTotalComment = await _httpClient.PutAsJsonAsync(userUrl, userDetail);
                if (updatePostData.IsSuccessStatusCode)
                {
                    commentDetails.Add(new ViewAllCommentsModel
                    {
                        userId = newComment.userId,
                        commentId = newComment.commentId,
                        commentPost = newComment.commentPost,
                        commentTimeStamp = newComment.commentTimeStamp,
                        avatar = userDetail.avatar,
                        fullName = userDetail.fullName
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