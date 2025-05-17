using System.Text.Json.Serialization;

namespace LoginRegistration.Model
{
    public class ViewAllPostsModel
    {
        //UserInteraction Model from Resource UserInteraction
        [JsonPropertyName("Posts")]
        public PostModel Posts { get; set; }

        [JsonPropertyName("UserDetailId")]
        public string UserDetailId { get; set; }

        public string postId { get; set; }

        //UserDetails From Resource UserDetails
        public string avatar { get; set; }

        public string fullName { get; set; }
    }
}