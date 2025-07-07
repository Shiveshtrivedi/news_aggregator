using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Exceptions
{
    public class LogoutException : Exception
    {
        public LogoutException() : base("User Requested Logout") { }
    }
}
