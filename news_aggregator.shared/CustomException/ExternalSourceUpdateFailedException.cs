using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.shared.CustomException
{
    public class ExternalSourceUpdateFailedException : Exception
    {
        public int StatusCode { get; }

        public ExternalSourceUpdateFailedException(string message) : base(message)
        {
            StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
}
