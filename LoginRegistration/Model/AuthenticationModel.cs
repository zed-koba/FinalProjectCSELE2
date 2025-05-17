namespace LoginRegistration.Model
{
    public class AuthenticationModel
    {
        public long dateCreated { get; set; }
        public string avatar { get; set; }
        public string fullName { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string emailAddress { get; set; }
        public int totalPosts { get; set; }
        public int totalComments { get; set; }
        public int totalLikes { get; set; }
        public string id { get; set; }
    }
}