using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.domain.Models.DTOs
{
    public class NewsArticleDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime PublishedAt { get; set; }
        public string Source { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int? ExternalSourceId { get; set; }
    }
}
