using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.shared.CustomException.NewsArticle
{
    public class NewsArticleNotFoundException : Exception
    {
        public int StatusCode { get; }

        public NewsArticleNotFoundException(string message = "Article not found.") : base(message)
        {
            StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
}
