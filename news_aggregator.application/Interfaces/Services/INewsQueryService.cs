using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application.Interfaces.Services
{
    public interface INewsQueryService
    {
        Task<IEnumerable<NewsArticle>> GetAllNewsAsync();
        Task<NewsArticle?> GetNewsByIdAsync(int id);
        Task<IEnumerable<NewsArticle>> SearchNewsByTitleAsync(string title);
        Task<IEnumerable<NewsArticle>> GetNewsByCategoryAsync(string category);
        Task<IEnumerable<NewsArticle>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate);

    }
}
