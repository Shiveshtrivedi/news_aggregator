using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Models
{
    public class SaveArticleDto
    {
        public int UserId { get; set; }
        public int ArticleId { get; set; }
    }
}
