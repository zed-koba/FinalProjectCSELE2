namespace LoginRegistration.Model
{
    public class ViewAllCommentsModel
    {
        //Default attributes
        public string userId { get; set; }

        public string commentId { get; set; }
        public string commentPost { get; set; }
        public long commentTimeStamp { get; set; }

        //UserDetails attribute needed

        public string avatar { get; set; }
        public string fullName { get; set; }
    }
}