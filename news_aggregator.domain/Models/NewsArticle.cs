using news_application.Enum;
using System.Text.Json.Serialization;

namespace news_application.Models
{
    public class NewsArticle
    {
        public int NewsArticleId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

        public string Url { get; set; } = string.Empty;
        public DateTime PublishedAt { get; set; }
        public CategoryType Category { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int? ExternalSourceId { get; set; }
        [JsonIgnore]
        public ExternalSource? ExternalSource { get; set; }
    }
}
