using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application
{
    public class SavedArticleService : ISavedArticleService
    {
        private readonly ISavedArticleRepository _savedArticleRepository;

        public SavedArticleService(ISavedArticleRepository savedArticleRepository)
        {
            _savedArticleRepository = savedArticleRepository;
        }

        public async Task SaveArticleAsync(int userId, int newsArticleId)
        {
            await _savedArticleRepository.SaveArticleAsync(userId, newsArticleId);
        }

        public async Task<IEnumerable<NewsArticle>> GetSavedArticlesByUserIdAsync(int userId)
        {
            return await _savedArticleRepository.GetSavedArticlesByUserIdAsync(userId);
        }

        public async Task DeleteSavedArticleAsync(int userId, int newsArticleId)
        {
            await _savedArticleRepository.DeleteSavedArticleAsync(userId, newsArticleId);
        }

    }
}
