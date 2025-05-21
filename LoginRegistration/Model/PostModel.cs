using System.Collections.ObjectModel;

namespace LoginRegistration.Model
{
    public class PostModel
    {
        public string imageSource { get; set; }
        public string caption { get; set; }
        public long timeStamp { get; set; }
        public ObservableCollection<CommentModel> comments { get; set; }
        public ObservableCollection<string> like { get; set; }
    }
}