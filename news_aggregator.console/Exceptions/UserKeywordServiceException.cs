using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Exceptions
{
    public class UserKeywordServiceException : Exception
    {
        public UserKeywordServiceException(string message) : base(message) { }
        public UserKeywordServiceException(string message, Exception innerException) : base(message, innerException) { }
    }

}
