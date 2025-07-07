using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Exceptions
{
    public class NewsServiceException : Exception
    {
        public NewsServiceException(string message) : base(message) { }
        public NewsServiceException(string message, Exception innerException) : base(message, innerException) { }
    }
}
