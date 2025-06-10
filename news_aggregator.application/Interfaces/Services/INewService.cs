using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application.Interfaces.Services
{
    public interface INewsService
    {
        Task<IEnumerable<NewsArticle>> FetchAndSaveExternalNewsAsync();
    }
}
