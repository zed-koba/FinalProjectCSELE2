using System.Collections.ObjectModel;

namespace LoginRegistration.Model
{
    public class PostModel
    {
        public string postId { get; set; }
        public string imageSource { get; set; }
        public string caption { get; set; }
        public string timeStamp { get; set; }
        public ObservableCollection<CommentModel> comments { get; set; }
        public ObservableCollection<LikeModel> like { get; set; }
        public int noOfComments { get; set; }
    }
}
