using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.domain.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application
{
    public class RecommendationService : IRecommendationService
    {
        private readonly INewsArticleRepository _newsRepo;
        private readonly ISavedArticleRepository _savedRepo;
        private readonly IUserArticleInteractionRepository _interactionRepo;

        public RecommendationService(
            INewsArticleRepository newsRepo,
            ISavedArticleRepository savedRepo,
            IUserArticleInteractionRepository interactionRepo)
        {
            _newsRepo = newsRepo;
            _savedRepo = savedRepo;
            _interactionRepo = interactionRepo;
        }

        public async Task<List<NewsArticleDto>> GetPersonalizedArticlesAsync(int userId)
        {
            var allArticles = await _newsRepo.GetAllAsync();
            var savedArticles = await _savedRepo.GetSavedArticlesByUserIdAsync(userId);
            var likedArticleIds = await _interactionRepo.GetLikedArticleIdsAsync(userId);

            var savedIds = savedArticles.Select(a => a.NewsArticleId).ToHashSet();

            var sorted = allArticles
                .OrderByDescending(a =>
                    savedIds.Contains(a.NewsArticleId) ? 2 :
                    likedArticleIds.Contains(a.NewsArticleId) ? 1 : 0)
                .Select(a => new NewsArticleDto
                {
                    NewsArticleId = a.NewsArticleId,
                    Title = a.Title,
                    Content = a.Content,
                    PublishedAt = a.PublishedAt,
                    ExternalSourceId = a.ExternalSourceId,
                    Category = a.Category.ToString(),
                    Source = a.Source,
                    Url = a.Url,
                    Likes = a.Likes,
                    DisLikes = a.Dislikes,
                    IsHidden = a.IsHidden,
                    ReportCount = a.ReportCount
                })
                .ToList();

            return sorted;
        }
    }
}
