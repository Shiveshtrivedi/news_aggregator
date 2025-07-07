using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.shared.CustomException.ExternalSource
{
    public class ExternalSourceNotFoundException : Exception
    {
        public int StatusCode { get; }

        public ExternalSourceNotFoundException(string message) : base(message)
        {
            StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
}
