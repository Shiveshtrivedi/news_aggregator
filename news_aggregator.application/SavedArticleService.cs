using AutoMapper;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.domain.Models.DTOs;
using news_application.Models;
using news_aggregator.shared.CustomException;

namespace news_aggregator.application
{
    public class SavedArticleService : ISavedArticleService
    {
        private readonly ISavedArticleRepository _savedArticleRepository;
        private readonly IMapper _mapper;

        public SavedArticleService(ISavedArticleRepository savedArticleRepository, IMapper mapper)
        {
            _savedArticleRepository = savedArticleRepository;
            _mapper = mapper;
        }

        public async Task SaveArticleAsync(int userId, int newsArticleId)
        {
            try
            {
                await _savedArticleRepository.SaveArticleAsync(userId, newsArticleId);
            }
            catch (SaveArticleFailedException)
            {
                throw;
            }
        }

        public async Task<IEnumerable<NewsArticleDto>> GetSavedArticlesByUserIdAsync(int userId)
        {
            try
            {
                var savedArticles = await _savedArticleRepository.GetSavedArticlesByUserIdAsync(userId);
                return _mapper.Map<IEnumerable<NewsArticleDto>>(savedArticles);
            }
            catch (GetSavedArticlesFailedException)
            {
                throw;
            }
        }

        public async Task DeleteSavedArticleAsync(int userId, int newsArticleId)
        {
            try
            {
                await _savedArticleRepository.DeleteSavedArticleAsync(userId, newsArticleId);
            }
            catch (DeleteSavedArticleFailedException)
            {
                throw;
            }
        }
    }
}
