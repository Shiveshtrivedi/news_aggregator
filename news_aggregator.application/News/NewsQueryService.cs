using AutoMapper;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.domain.Models.DTOs;
using news_aggregator.shared.CustomException;
using news_aggregator.shared.CustomException.NewsArticle;
using news_application.Enum;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application.News
{
    public class NewsQueryService : INewsQueryService
    {
        private readonly INewsArticleRepository _newsArticleRepository;
        private readonly IUserArticleInteractionRepository _userArticleInteractionRepository;
        private readonly IReportArticleRepository _reportArticleRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBlockedKeywordService _blockedKeywordService;
        private readonly  IUserKeywordService _userKeywordService;
        private readonly  ISavedArticleRepository _savedArticleRepository;
        private readonly IMapper _mapper;

        public NewsQueryService(INewsArticleRepository newsArticleRepository,
                                IUserArticleInteractionRepository userArticleInteractionRepository,
                                IMapper mapper,
                                IReportArticleRepository reportArticleRepository,
                                ICategoryRepository categoryRepository,
                                IBlockedKeywordService blockedKeywordService,
                                IUserKeywordService userKeywordService,
                                ISavedArticleRepository savedArticleRepository)
        {
            _newsArticleRepository = newsArticleRepository;
            _userArticleInteractionRepository = userArticleInteractionRepository;
            _mapper = mapper;
            _reportArticleRepository = reportArticleRepository;
            _categoryRepository = categoryRepository;
            _blockedKeywordService = blockedKeywordService;
            _userKeywordService = userKeywordService;
            _savedArticleRepository = savedArticleRepository;
        }

        public async Task<IEnumerable<NewsArticleDto>> GetAllNewsAsync()
        {
            var articles = await _newsArticleRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<NewsArticleDto>>(articles);
        }

        public async Task<NewsArticleDto?> GetNewsByIdAsync(int articleId)
        {
            var article = await _newsArticleRepository.GetByIdAsync(articleId);
            if (article == null)
                throw new NewsArticleNotFoundException($"Article with ID {articleId} not found.");

            return _mapper.Map<NewsArticleDto>(article);
        }

        public async Task<IEnumerable<NewsArticleDto>> SearchNewsByTitleAsync(string title, DateTime? startDate, DateTime? endDate)
        {
            var articles = await _newsArticleRepository.SearchNewsByTitleAsync(title, startDate, endDate);
            return _mapper.Map<IEnumerable<NewsArticleDto>>(articles);
        }

        public async Task<IEnumerable<NewsArticleDto>> GetNewsByCategoryAsync(string category)
        {

            var articles = await _newsArticleRepository.GetNewsByCategoryAsync(category);
            return _mapper.Map<IEnumerable<NewsArticleDto>>(articles);
        }
        public async Task<IEnumerable<NewsArticleDto>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var articles = await _newsArticleRepository.GetNewsByDateRangeAsync(startDate, endDate);
            return _mapper.Map<IEnumerable<NewsArticleDto>>(articles);
        }

        public async Task<List<NewsArticleWithUserInteractionDto>> GetNewsByCategoryAndDateRangeAsync(
     string category, DateTime? startDate, DateTime? endDate, int userId)
        {
            var articleDtos = await _newsArticleRepository.GetNewsByCategoryAndDateRangeAsync(category, startDate, endDate);
            var userKeywords = await _userKeywordService.GetKeywordsAsync(userId);
            var likedArticleIds = await _userArticleInteractionRepository.GetLikedArticleIdsAsync(userId);
            var savedArticles = await _savedArticleRepository.GetSavedArticlesByUserIdAsync(userId);
            var savedIds = savedArticles.Select(x => x.NewsArticleId).ToHashSet();
            var likedIds = likedArticleIds.ToHashSet();

            var personalizedList = new List<(NewsArticleWithUserInteractionDto Article, int Score)>();

            foreach (var article in articleDtos)
            {
                // Category check
                var articleCategory = await _categoryRepository.GetCategoryByNameAsync(article.Category);
                if (articleCategory?.IsHidden == true)
                    continue;

                // Blocked keyword check
                if (await _blockedKeywordService.ContainsBlockedKeywordAsync(article.Title) ||
                    await _blockedKeywordService.ContainsBlockedKeywordAsync(article.Content))
                    continue;

                // Report count / Hidden check
                var reportCount = await _reportArticleRepository.GetReportCountAsync(article.NewsArticleId);
                if (reportCount > 3 || article.IsHidden)
                    continue;

                // User interaction
                var interaction = await _userArticleInteractionRepository.GetInteractionAsync(userId, article.NewsArticleId);
                bool isLiked = interaction?.IsLiked ?? false;
                bool isDisliked = interaction?.IsDisliked ?? false;

                // Personalized score
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

                var dto = new NewsArticleWithUserInteractionDto
                {
                    NewsArticleId = article.NewsArticleId,
                    Title = article.Title,
                    Content = article.Content,
                    Url = article.Url,
                    Source = article.Source,
                    Category = article.Category,
                    PublishedAt = article.PublishedAt,
                    IsLikedByUser = isLiked,
                    IsDislikedByUser = isDisliked,
                    Likes = article.Likes,
                    Dislikes = article.DisLikes
                };

                personalizedList.Add((dto, score));
            }

            // Sort by descending score
            return personalizedList
                .OrderByDescending(x => x.Score)
                .Select(x => x.Article)
                .ToList();
        }

    }
}
