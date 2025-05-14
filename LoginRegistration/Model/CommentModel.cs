using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginRegistration.Model
{
    public class CommentModel
    {
        public string userId {  get; set; }
        public string commentId { get; set; }
        public string commentPost {  get; set; }
        public string commentTimeStamp { get; set; }
    }
}
