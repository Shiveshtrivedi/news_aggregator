using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.shared.CustomException.UserException
{
    public class UserAlreadyExistsException : Exception
    {
        public int StatusCode { get; }

        public UserAlreadyExistsException(string message = "User already exists.") : base(message)
        {
            StatusCode = (int)HttpStatusCode.Conflict;
        }
    }
}
