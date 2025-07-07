using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.shared.CustomException.UserException
{
    public class InvalidCredentialsException : Exception
    {
        public int StatusCode { get; }

        public InvalidCredentialsException(string message = "Invalid email or password.") : base(message)
        {
            StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}
