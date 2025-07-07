using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using news_aggregator.console.Models;

namespace news_aggregator.console.Services.Interfaces
{
    public interface INewsService
    {
        Task<List<NewsArticleDto>> GetArticlesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<List<NewsArticleDto>> GetNewsByCategoryAndDateRangeAsync(string category, DateTime startDate, DateTime endDate);
        Task<bool> LikeArticleAsync(int articleId);
        Task<bool> DislikeArticleAsync(int articleId);
        Task<bool> ReportArticleAsync(int articleId , string message);
        
        }
}
