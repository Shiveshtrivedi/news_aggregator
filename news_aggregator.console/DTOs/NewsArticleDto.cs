using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace news_aggregator.console.Models
{
    public class NewsArticleDto
    {
        public int? NewsArticleId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CategoryType? Category { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Source { get; set; } = string.Empty;
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public bool IsLikedByUser { get; set; }
        public bool IsDislikedByUser { get; set; }
    }

    public enum CategoryType
    {
        business = 0,
        entertainment = 1,
        sports = 2,
        uncategorized = 4,
        technology = 5,
        general = 6,
        health = 7
    }

}
