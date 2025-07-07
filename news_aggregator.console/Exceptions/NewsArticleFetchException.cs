using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Exceptions
{
    public class NewsArticleFetchException : Exception
    {
        public NewsArticleFetchException(string message) : base(message) { }

        public NewsArticleFetchException(string message, Exception innerException) : base(message, innerException) { }
    }
}
