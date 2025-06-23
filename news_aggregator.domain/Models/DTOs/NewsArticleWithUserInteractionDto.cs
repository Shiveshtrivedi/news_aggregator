using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.domain.Models.DTOs
{
    public class NewsArticleWithUserInteractionDto
    {
        public int NewsArticleId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Content { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public string? Category { get; set; }
        public string Source { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public DateTime PublishedAt { get; set; }

        public bool IsLikedByUser { get; set; }
        public bool IsDislikedByUser { get; set; }
    }

}
