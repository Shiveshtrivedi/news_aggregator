using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Exceptions
{
    public class NotificationServiceException : Exception
    {
        public NotificationServiceException(string message) : base(message) { }
        public NotificationServiceException(string message, Exception innerException) : base(message, innerException) { }
    }
}
