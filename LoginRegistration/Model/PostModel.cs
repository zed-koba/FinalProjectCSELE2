using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
