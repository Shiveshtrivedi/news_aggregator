using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Exceptions
{
    public class SavedArticleException : Exception
    {
        public SavedArticleException(string message) : base(message) { }

        public SavedArticleException(string message, Exception innerException) : base(message, innerException) { }
    }
}
