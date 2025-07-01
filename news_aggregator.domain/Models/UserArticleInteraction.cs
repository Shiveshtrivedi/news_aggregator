using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.domain.Models
{
    public class UserArticleInteraction
    {
        public int UserArticleInteractionId { get; set; }
        public int UserId { get; set; }
        public int NewsArticleId { get; set; }

        public bool IsLiked { get; set; }
        public bool IsDisliked { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public NewsArticle? NewsArticle { get; set; }
        public User? User { get; set; } 
    }

}
