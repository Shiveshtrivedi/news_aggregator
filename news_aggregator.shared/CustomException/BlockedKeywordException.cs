using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.shared.CustomException
{
    public class BlockedKeywordOperationException : Exception
    {
        public BlockedKeywordOperationException() { }

        public BlockedKeywordOperationException(string message) : base(message) { }

        public BlockedKeywordOperationException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
