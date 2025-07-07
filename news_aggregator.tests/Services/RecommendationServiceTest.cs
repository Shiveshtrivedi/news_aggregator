using Moq;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.tests.Helpers;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.application.Article;

namespace news_aggregator.tests.Services
{
    public class RecommendationServiceTest
    {
        private readonly Mock<INewsArticleRepository> _newsRepoMock;
        private readonly Mock<ISavedArticleRepository> _savedRepoMock;
        private readonly Mock<IUserArticleInteractionRepository> _interactionRepoMock;
        private readonly Mock<IUserKeywordService> _userKeywordServiceMock;

        private readonly RecommendationService _service;

        public RecommendationServiceTest()
        {
            _newsRepoMock = new Mock<INewsArticleRepository>();
            _savedRepoMock = new Mock<ISavedArticleRepository>();
            _interactionRepoMock = new Mock<IUserArticleInteractionRepository>();
            _userKeywordServiceMock = new Mock<IUserKeywordService>();

            _service = new RecommendationService(
                _newsRepoMock.Object,
                _savedRepoMock.Object,
                _interactionRepoMock.Object,
                _userKeywordServiceMock.Object
            );
        }

        [Fact]
        public async Task ShouldScoreHigherForSavedArticles()
        {
            int userId = 1;
            var allArticles = RecommendationMockData.GetAllArticles();
            var savedArticles = RecommendationMockData.GetSavedArticles();

            _newsRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(allArticles);
            _savedRepoMock.Setup(r => r.GetSavedArticlesByUserIdAsync(userId)).ReturnsAsync(savedArticles);
            _interactionRepoMock.Setup(r => r.GetLikedArticleIdsAsync(userId)).ReturnsAsync(new List<int>());
            _userKeywordServiceMock.Setup(r => r.GetKeywordsAsync(userId)).ReturnsAsync(new List<string>());

            var result = await _service.GetPersonalizedArticlesAsync(userId);

            Assert.Equal(3, result.Count);
            Assert.Equal(savedArticles[0].NewsArticleId, result.First().NewsArticleId);
        }

        [Fact]
        public async Task ShouldScoreHigherForLikedArticles()
        {
            int userId = 2;
            var allArticles = RecommendationMockData.GetAllArticles();

            _newsRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(allArticles);
            _savedRepoMock.Setup(r => r.GetSavedArticlesByUserIdAsync(userId)).ReturnsAsync(new List<NewsArticle>());
            _interactionRepoMock.Setup(r => r.GetLikedArticleIdsAsync(userId)).ReturnsAsync(RecommendationMockData.GetLikedArticleIds());
            _userKeywordServiceMock.Setup(r => r.GetKeywordsAsync(userId)).ReturnsAsync(new List<string>());

            var result = await _service.GetPersonalizedArticlesAsync(userId);

            Assert.Equal(3, result.Count);
            Assert.Equal(1, result.First().NewsArticleId); 
        }

        [Fact]
        public async Task ShouldScoreHigherForKeywordMatch()
        {
            int userId = 3;
            var allArticles = RecommendationMockData.GetAllArticles();

            _newsRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(allArticles);
            _savedRepoMock.Setup(r => r.GetSavedArticlesByUserIdAsync(userId)).ReturnsAsync(new List<NewsArticle>());
            _interactionRepoMock.Setup(r => r.GetLikedArticleIdsAsync(userId)).ReturnsAsync(new List<int>());
            _userKeywordServiceMock.Setup(r => r.GetKeywordsAsync(userId)).ReturnsAsync(RecommendationMockData.GetUserKeywords());

            var result = await _service.GetPersonalizedArticlesAsync(userId);

            Assert.Equal(3, result.Count);
            Assert.Contains(result, r => r.Title.Contains("AI") || r.Title.Contains("climate", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public async Task ShouldReturnSortedArticlesByScore()
        {
            int userId = 4;

            _newsRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(RecommendationMockData.GetAllArticles());
            _savedRepoMock.Setup(r => r.GetSavedArticlesByUserIdAsync(userId)).ReturnsAsync(RecommendationMockData.GetSavedArticles());
            _interactionRepoMock.Setup(r => r.GetLikedArticleIdsAsync(userId)).ReturnsAsync(RecommendationMockData.GetLikedArticleIds());
            _userKeywordServiceMock.Setup(r => r.GetKeywordsAsync(userId)).ReturnsAsync(RecommendationMockData.GetUserKeywords());

            var result = await _service.GetPersonalizedArticlesAsync(userId);

            Assert.Equal(3, result.Count);
            Assert.True(result[0].Title.Contains("Climate") || result[0].Title.Contains("AI"));
        }

        [Fact]
        public async Task ShouldReturnEmptyListIfNoArticles()
        {
            int userId = 5;

            _newsRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<NewsArticle>());
            _savedRepoMock.Setup(r => r.GetSavedArticlesByUserIdAsync(userId)).ReturnsAsync(new List<NewsArticle>());
            _interactionRepoMock.Setup(r => r.GetLikedArticleIdsAsync(userId)).ReturnsAsync(new List<int>());
            _userKeywordServiceMock.Setup(r => r.GetKeywordsAsync(userId)).ReturnsAsync(new List<string>());

            var result = await _service.GetPersonalizedArticlesAsync(userId);

            Assert.Empty(result);
        }
    }
}
