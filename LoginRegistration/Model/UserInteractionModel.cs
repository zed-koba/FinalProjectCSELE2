using System.Text.Json.Serialization;

namespace LoginRegistration.Model
{
    public class UserInteractionModel
    {
        [JsonPropertyName("Posts")]
        public PostModel Posts { get; set; }

        [JsonPropertyName("UserDetailId")]
        public string UserDetailId { get; set; }

        public string postId { get; set; }
    }
}