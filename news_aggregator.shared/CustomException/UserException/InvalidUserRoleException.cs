using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.shared.CustomException.UserException
{
    public class InvalidUserRoleException : Exception
    {
        public int StatusCode { get; }

        public InvalidUserRoleException(string message = "Invalid user role. Allowed values are: 0 (Admin), 1 (User).")
        {
            StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
}
