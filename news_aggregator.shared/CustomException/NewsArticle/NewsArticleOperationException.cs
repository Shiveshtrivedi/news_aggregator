using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.shared.CustomException.NewsArticle
{
    public class NewsArticleOperationException : Exception
    {
        public NewsArticleOperationException(string message) : base(message) { }
        public NewsArticleOperationException(string message, Exception inner) : base(message, inner) { }
    }
}
