using news_aggregator.console.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Services.Interfaces
{
    public interface INewsArticleService
    {
        Task<List<NewsArticleDto>> GetNewsByCategoryAndDateRangeAsync(string category, DateTime start, DateTime end);
        Task SaveArticleAsync(string articleId);
    }
}
