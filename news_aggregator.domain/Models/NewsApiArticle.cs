using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.domain.Models
{
    public class NewsApiArticle
    {
        public SourceInfo Source { get; set; } = new();
        public string Author { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string UrlToImage { get; set; } = string.Empty;
        public DateTime PublishedAt { get; set; }
        public string Content { get; set; } = string.Empty;
    }

    public class SourceInfo
    {
        public string? Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

}
