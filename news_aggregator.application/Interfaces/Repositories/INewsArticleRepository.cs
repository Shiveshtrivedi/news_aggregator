using news_aggregator.domain.Models.DTOs;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application.Interfaces.Repositories
{
    public interface INewsArticleRepository
    {
        Task<IEnumerable<NewsArticle>> GetAllAsync();
        Task<NewsArticle?> GetByIdAsync(int id);
        Task AddAsync(NewsArticle article);
        Task DeleteAsync(int newArticleId);
        Task DeleteOlderThanAsync(DateTime cutoffDate);
        Task<bool> ExistsAsync(string title);
        Task<IEnumerable<NewsArticle>> SearchNewsByTitleAsync(string title);
        Task<IEnumerable<NewsArticle>> GetNewsByCategoryAsync(string category);
        Task<IEnumerable<NewsArticle>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
