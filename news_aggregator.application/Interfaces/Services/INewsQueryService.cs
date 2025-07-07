using news_aggregator.domain.Models.DTOs;
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
        Task<IEnumerable<NewsArticleDto>> GetAllNewsAsync();
        Task<NewsArticleDto?> GetNewsByIdAsync(int articleId);
        Task<IEnumerable<NewsArticleDto>> SearchNewsByTitleAsync(string title, DateTime? startDate, DateTime? endDate);
        Task<IEnumerable<NewsArticleDto>> GetNewsByCategoryAsync(string category);
        Task<IEnumerable<NewsArticleDto>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<NewsArticleWithUserInteractionDto>> GetNewsByCategoryAndDateRangeAsync(string category, DateTime? startDate, DateTime? endDate,int userId);


    }
}
