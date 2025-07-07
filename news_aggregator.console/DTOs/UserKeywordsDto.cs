using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.DTOs
{
    public class UserKeywordsDto
    {
        public IEnumerable<string> Keywords { get; set; } = new List<string>();
    }

    public class UserKeywordDto
    {
        public string Keywords { get; set; } = string.Empty;
    }
}
