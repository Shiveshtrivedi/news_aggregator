using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Exceptions
{
    public class SearchArticleException : Exception
    {
        public SearchArticleException(string message) : base(message) { }
        public SearchArticleException(string message, Exception innerException) : base(message, innerException) { }
    }
}
