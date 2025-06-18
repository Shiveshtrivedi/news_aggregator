using System.Text.Json.Serialization;

namespace news_application.Models
{
    public class NotificationConfig
    {
        public int NotificationConfigId { get; set; }
        public int UserId { get; set; }

        public bool KeywordsEnabled { get; set; }

        public User User { get; set; } = null!;
        public ICollection<NotificationCategorySetting> CategorySettings { get; set; } = new List<NotificationCategorySetting>();
    }

    public class NotificationCategorySetting
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }
        [JsonIgnore]
        public NotificationConfig NotificationConfig { get; set; } = null!;
    }

}
