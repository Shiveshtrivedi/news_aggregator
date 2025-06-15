using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application
{
    public interface INewsApiParser
    {
        bool CanHandle(ExternalSource source);
        Task<IEnumerable<NewsArticle>> FetchArticlesAsync(ExternalSource source, string category, string keyword);
    }
}
