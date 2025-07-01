using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.domain.Models
{
    public class UserKeyword
    {
        public int UserKeywordId { get; set; }
        public int UserId { get; set; }
        public string Keyword { get; set; } = string.Empty;
        public User? User { get; set; }
    }
}
