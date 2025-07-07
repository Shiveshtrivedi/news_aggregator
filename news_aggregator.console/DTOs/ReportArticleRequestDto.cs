using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.DTOs
{
    public class ReportArticleRequestDto
    {
        public int ArticleId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
