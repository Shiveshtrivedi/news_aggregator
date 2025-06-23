using AutoMapper;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.domain.Models.DTOs;
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
        private readonly IMapper _mapper;

        public SavedArticleService(ISavedArticleRepository savedArticleRepository, IMapper mapper)
        {
            _savedArticleRepository = savedArticleRepository;
            _mapper = mapper;
        }

        public async Task SaveArticleAsync(int userId, int newsArticleId)
        {
            await _savedArticleRepository.SaveArticleAsync(userId, newsArticleId);
        }

        public async Task<IEnumerable<NewsArticleDto>> GetSavedArticlesByUserIdAsync(int userId)
        {
            var savedArticles = await _savedArticleRepository.GetSavedArticlesByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<NewsArticleDto>>(savedArticles);
        }

        public async Task DeleteSavedArticleAsync(int userId, int newsArticleId)
        {
            await _savedArticleRepository.DeleteSavedArticleAsync(userId, newsArticleId);
        }

    }
}
