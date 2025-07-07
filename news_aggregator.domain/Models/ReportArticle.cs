using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.domain.Models
{
    public class ReportArticle
    {
        public int ReportArticleId { get; set; }
        public int UserId { get; set; }
        public int NewsArticleId { get; set; }
        public string Message { get; set; } = string.Empty;
        public NewsArticle? NewsArticle { get; set; }
        public User? User { get; set; }
        public DateTime ReportedAt { get; set; } = DateTime.UtcNow;
    }
}
