namespace news_application.Models
{
    public class NotificationConfig
    {
        public int NotificationConfigId { get; set; }
        public int UserId { get; set; }

        public bool BusinessEnabled { get; set; }
        public bool EntertainmentEnabled { get; set; }
        public bool SportsEnabled { get; set; }
        public bool TechnologyEnabled { get; set; }
        public bool KeywordsEnabled { get; set; }

        public User User { get; set; } = null!;
    }
}
