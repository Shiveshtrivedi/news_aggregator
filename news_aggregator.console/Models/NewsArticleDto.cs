using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Models
{
    public class NewsArticleDto
    {
        public int? NewsArticleId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
        public CategoryType? Category { get; set; }
        public DateTime PublishedAt { get; set; }
        public string Source { get; set; } = string.Empty;
    }

    public enum CategoryType
    {
        Business = 0,
        Entertainment = 1,
        Sports = 2,
        Technology = 3,
        Uncategorized = 4
    }

}
