using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Exceptions
{
    public class ServerServiceException : Exception
    {
        public ServerServiceException(string message) : base(message) { }
        public ServerServiceException(string message, Exception innerException) : base(message, innerException) { }
    }
}
