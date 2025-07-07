using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.infrastructure.Adapter.ExternalNews.Interface
{
    public interface INewsApiResponseParser
    {
        IEnumerable<NewsArticle> Parse(string rawJson, string sourceName, string category);
    }

}
