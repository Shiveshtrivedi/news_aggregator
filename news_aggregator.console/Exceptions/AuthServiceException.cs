using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Exceptions
{
    public class AuthServiceException : Exception
    {
        public AuthServiceException(string message) : base(message) { }
        public AuthServiceException(string message, Exception innerException) : base(message, innerException) { }
    }
}
