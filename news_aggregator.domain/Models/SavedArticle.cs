namespace news_application.Models
{
    public class SavedArticle
    {
        public int SavedArticleId { get; set; }
        public int UserId { get; set; }
        public int NewsArticleId { get; set; }
        public DateTime SavedOn { get; set; }

        public User User { get; set; } = null!;
        public NewsArticle NewsArticle { get; set; } = null!;

    }
}
