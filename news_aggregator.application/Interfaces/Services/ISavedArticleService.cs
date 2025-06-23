using news_aggregator.domain.Models.DTOs;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application.Interfaces.Services
{
    public interface ISavedArticleService
    {
        Task SaveArticleAsync(int userId, int newsArticleId);
        Task<IEnumerable<NewsArticleDto>> GetSavedArticlesByUserIdAsync(int userId);
        Task DeleteSavedArticleAsync(int userId, int articleId);
    }
}
