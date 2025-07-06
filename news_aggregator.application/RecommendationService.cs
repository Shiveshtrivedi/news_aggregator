using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.domain.Models.DTOs;

namespace news_aggregator.application
{
    public class RecommendationService : IRecommendationService
    {
        private readonly INewsArticleRepository _newsRepo;
        private readonly ISavedArticleRepository _savedRepo;
        private readonly IUserArticleInteractionRepository _interactionRepo;
        private readonly IUserKeywordService _userKeywordService;

        public RecommendationService(
            INewsArticleRepository newsRepo,
            ISavedArticleRepository savedRepo,
            IUserArticleInteractionRepository interactionRepo,
            IUserKeywordService userKeywordService)
        {
            _newsRepo = newsRepo;
            _savedRepo = savedRepo;
            _interactionRepo = interactionRepo;
            _userKeywordService = userKeywordService;
        }

        public async Task<List<NewsArticleDto>> GetPersonalizedArticlesAsync(int userId)
        {
            var allArticles = await _newsRepo.GetAllAsync();
            var savedArticles = await _savedRepo.GetSavedArticlesByUserIdAsync(userId);
            var likedArticleIds = await _interactionRepo.GetLikedArticleIdsAsync(userId);
            var userKeywords = await _userKeywordService.GetKeywordsAsync(userId);

            var savedIds = savedArticles.Select(a => a.NewsArticleId).ToHashSet();
            var likedIds = likedArticleIds.ToHashSet();

            var sorted = allArticles
                .Select(article =>
                {
                    int score = 0;

                    if (savedIds.Contains(article.NewsArticleId)) score += 50;
                    else if (likedIds.Contains(article.NewsArticleId)) score += 30;

                    foreach (var keyword in userKeywords)
                    {
                        if (!string.IsNullOrWhiteSpace(keyword) &&
                            (article.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                             article.Content.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
                        {
                            score += 10;
                        }
                    }

                    return new
                    {
                        Article = new NewsArticleDto
                        {
                            NewsArticleId = article.NewsArticleId,
                            Title = article.Title,
                            Content = article.Content,
                            PublishedAt = article.PublishedAt,
                            ExternalSourceId = article.ExternalSourceId,
                            Category = article.Category.ToString(),
                            Source = article.Source,
                            Url = article.Url,
                            Likes = article.Likes,
                            DisLikes = article.Dislikes,
                            IsHidden = article.IsHidden,
                            ReportCount = article.ReportCount
                        },
                        Score = score
                    };
                })
                .OrderByDescending(x => x.Score)
                .Select(x => x.Article)
                .ToList();

            return sorted;
        }
    }
}
