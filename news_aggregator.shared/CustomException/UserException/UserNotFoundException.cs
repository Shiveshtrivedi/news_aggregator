using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.shared.CustomException.UserException
{
    public class UserNotFoundException : Exception
    {
        public int StatusCode { get; }

        public UserNotFoundException(string message = "User not found.") : base(message)
        {
            StatusCode = (int)HttpStatusCode.NotFound;
        }
    }

}

