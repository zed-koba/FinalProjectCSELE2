using System.Globalization;

namespace LoginRegistration.Features
{
    public class TimeStampConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "";

            if (long.TryParse(value.ToString(), out long unixTime))
            {
                var postDateUtc = DateTimeOffset.FromUnixTimeSeconds(unixTime).UtcDateTime;
                var postDate = postDateUtc.ToLocalTime(); // Converts to your local timezone
                var now = DateTime.Now;

                var diff = now - postDate;

                if (diff.TotalSeconds < 60)
                    return "just now";
                else if (diff.TotalMinutes < 60)
                    return $"{(int)diff.TotalMinutes} mins ago";
                else if (diff.TotalHours < 24)
                    return $"{(int)diff.TotalHours} hours ago";
                else if (diff.TotalDays < 7)
                    return $"{(int)diff.TotalDays} days ago";
                else
                    return postDate.ToString("MMMM dd, yyyy");
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}