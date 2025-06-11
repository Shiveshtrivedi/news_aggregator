using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using news_application.Models;

namespace news_aggregator.application.Interfaces.Repositories
{
    public interface ISavedArticleRepository
    {
        Task SaveArticleAsync(int userId, int newsArticleId);
        Task<IEnumerable<NewsArticle>> GetAllSavedArticlesAsync();
        Task<IEnumerable<NewsArticle>> GetSavedArticlesByUserIdAsync(int userId);
        Task DeleteSavedArticleAsync(int userId, int articleId);
    }
}
