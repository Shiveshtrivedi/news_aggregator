using news_aggregator.domain.Models.DTOs;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application.Interfaces.Repositories
{
    public interface INewsArticleRepository : IGenericRepository<NewsArticle>
    {
        Task<bool> ExistsAsync(string title, string url);
        Task<IEnumerable<NewsArticle>> SearchNewsByTitleAsync(string title, DateTime? startDate, DateTime? endDate);
        Task<IEnumerable<NewsArticle>> GetNewsByCategoryAsync(string category);
        Task<IEnumerable<NewsArticle>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task DeleteOlderThanAsync(DateTime cutoffDate);
        Task<List<NewsArticleDto>> GetNewsByCategoryAndDateRangeAsync(string category, DateTime? startDate, DateTime? endDate);
        Task<NewsArticle?> GetByIdAsync(int articleId);
        Task IncrementLikesAsync(int articleId);
        Task DecrementLikesAsync(int articleId);
        Task IncrementDislikesAsync(int articleId);
        Task DecrementDislikesAsync(int articleId);
    }
}
