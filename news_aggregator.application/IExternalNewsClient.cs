using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application
{
    public interface IExternalNewsClient
    {
        //Task<IEnumerable<NewsArticle>> GetLatestArticlesAsync();
        Task<IEnumerable<NewsArticle>> GetLatestArticlesAsync(string category = "", string keyword = "");
    }
}
