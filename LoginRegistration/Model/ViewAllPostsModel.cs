using System.ComponentModel;
using System.Text.Json.Serialization;

namespace LoginRegistration.Model
{
    public class ViewAllPostsModel : INotifyPropertyChanged
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

        //Like Color Properties
        private string _likeColor;

        public string likeColor
        {
            get => _likeColor;
            set
            {
                if (_likeColor != value)
                {
                    _likeColor = value;
                    OnPropertyChanged(nameof(likeColor));
                }
            }
        }

        private string _iconFont;

        public string iconFont
        {
            get => _iconFont;
            set
            {
                if (_iconFont != value)
                {
                    _iconFont = value;
                    OnPropertyChanged(nameof(iconFont));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}