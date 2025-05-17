using System.Text.Json.Serialization;

namespace LoginRegistration.Model
{
    public class UserInteractionModel
    {
        //UserInteraction Model from Resource UserInteraction
        [JsonPropertyName("Posts")]
        public PostModel Posts { get; set; }

        [JsonPropertyName("UserDetailId")]
        public string UserDetailId { get; set; }

        public string postId { get; set; }
    }
}