using Xunit;
using Moq;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using news_aggregator.application;
using news_aggregator.domain.Models;
using news_aggregator.domain.Models.DTOs;
using news_aggregator.shared.CustomException.NewsArticle;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.tests.Helpers;
using news_application.Models;
using news_application.Enum;
using System.Linq;

namespace news_aggregator.tests.Services
{
    public class NewsQueryServiceTest
    {
        private readonly Mock<INewsArticleRepository> _articleRepoMock;
        private readonly Mock<IUserArticleInteractionRepository> _interactionRepoMock;
        private readonly Mock<IReportArticleRepository> _reportRepoMock;
        private readonly Mock<ICategoryRepository> _categoryRepoMock;
        private readonly Mock<IBlockedKeywordService> _blockedKeywordServiceMock;
        private readonly Mock<IUserKeywordService> _userKeywordServiceMock;
        private readonly Mock<ISavedArticleRepository> _savedArticleRepoMock;
        private readonly Mock<IMapper> _mapperMock;

        private readonly NewsQueryService _service;

        public NewsQueryServiceTest()
        {
            _articleRepoMock = new Mock<INewsArticleRepository>();
            _interactionRepoMock = new Mock<IUserArticleInteractionRepository>();
            _reportRepoMock = new Mock<IReportArticleRepository>();
            _categoryRepoMock = new Mock<ICategoryRepository>();
            _blockedKeywordServiceMock = new Mock<IBlockedKeywordService>();
            _userKeywordServiceMock = new Mock<IUserKeywordService>();
            _savedArticleRepoMock = new Mock<ISavedArticleRepository>();
            _mapperMock = new Mock<IMapper>();

            _service = new NewsQueryService(
                _articleRepoMock.Object,
                _interactionRepoMock.Object,
                _mapperMock.Object,
                _reportRepoMock.Object,
                _categoryRepoMock.Object,
                _blockedKeywordServiceMock.Object,
                _userKeywordServiceMock.Object,
                _savedArticleRepoMock.Object
            );

            _userKeywordServiceMock
     .Setup(s => s.GetKeywordsAsync(It.IsAny<int>()))
     .ReturnsAsync(new List<string>());


            _savedArticleRepoMock
     .Setup(s => s.GetSavedArticlesByUserIdAsync(It.IsAny<int>()))
     .ReturnsAsync(new List<NewsArticle>());

        }

        [Fact]
        public async Task GetAllNewsAsync_ShouldReturnMappedArticles()
        {
            var articles = new List<NewsArticle> { new() { NewsArticleId = 1, Title = "Test" } };
            var expectedDtos = NewsArticleDtoMockDataGenerator.GetNewsArticleDtoList();

            _articleRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(articles);
            _mapperMock.Setup(m => m.Map<IEnumerable<NewsArticleDto>>(articles)).Returns(expectedDtos);

            var result = await _service.GetAllNewsAsync();

            Assert.Equal(expectedDtos.Count, result.Count());
        }

        [Fact]
        public async Task GetNewsByIdAsync_ShouldReturnMappedDto_WhenFound()
        {
            var article = new NewsArticle { NewsArticleId = 1, Title = "Sample" };
            var dto = NewsArticleDtoMockDataGenerator.GetNewsArticleDto();

            _articleRepoMock.Setup(r => r.GetByIdAsync(article.NewsArticleId)).ReturnsAsync(article);
            _mapperMock.Setup(m => m.Map<NewsArticleDto>(article)).Returns(dto);

            var result = await _service.GetNewsByIdAsync(article.NewsArticleId);

            Assert.Equal(dto.NewsArticleId, result?.NewsArticleId);
        }

        [Fact]
        public async Task GetNewsByIdAsync_ShouldThrowException_WhenNotFound()
        {
            _articleRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((NewsArticle)null!);

            var action = async () => await _service.GetNewsByIdAsync(999);

            await Assert.ThrowsAsync<NewsArticleNotFoundException>(action);
        }

        [Fact]
        public async Task SearchNewsByTitleAsync_ShouldReturnMappedResults()
        {
            var articles = new List<NewsArticle> { new() { NewsArticleId = 1, Title = "test" } };
            var expected = NewsArticleDtoMockDataGenerator.GetNewsArticleDtoList();

            _articleRepoMock.Setup(r => r.SearchNewsByTitleAsync("test", null, null)).ReturnsAsync(articles);
            _mapperMock.Setup(m => m.Map<IEnumerable<NewsArticleDto>>(articles)).Returns(expected);

            var result = await _service.SearchNewsByTitleAsync("test", null, null);

            Assert.Equal(expected.Count, result.Count());
        }

        [Fact]
        public async Task GetNewsByCategoryAsync_ShouldReturnMappedResults()
        {
            var articles = new List<NewsArticle> { new() { NewsArticleId = 2, Category = CategoryType.technology } };
            var expected = NewsArticleDtoMockDataGenerator.GetNewsArticleDtoList();

            _articleRepoMock.Setup(r => r.GetNewsByCategoryAsync("technology")).ReturnsAsync(articles);
            _mapperMock.Setup(m => m.Map<IEnumerable<NewsArticleDto>>(articles)).Returns(expected);

            var result = await _service.GetNewsByCategoryAsync("technology");

            Assert.Equal(expected.Count, result.Count());
        }

        [Fact]
        public async Task GetNewsByDateRangeAsync_ShouldReturnMappedResults()
        {
            var articles = new List<NewsArticle> { new() { NewsArticleId = 3 } };
            var expected = NewsArticleDtoMockDataGenerator.GetNewsArticleDtoList();

            _articleRepoMock.Setup(r => r.GetNewsByDateRangeAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>())).ReturnsAsync(articles);
            _mapperMock.Setup(m => m.Map<IEnumerable<NewsArticleDto>>(articles)).Returns(expected);

            var result = await _service.GetNewsByDateRangeAsync(DateTime.UtcNow.AddDays(-5), DateTime.UtcNow);

            Assert.Equal(expected.Count, result.Count());
        }

        [Fact]
        public async Task GetNewsByCategoryAndDateRangeAsync_ShouldFilterBlockedKeywords()
        {
            var newsList = NewsArticleDtoMockDataGenerator.GetNewsArticleDtoList();
            newsList[0].Title = "Blocked title";

            _articleRepoMock.Setup(r => r.GetNewsByCategoryAndDateRangeAsync("technology", null, null)).ReturnsAsync(newsList);
            _categoryRepoMock.Setup(r => r.GetCategoryByNameAsync(It.IsAny<string>())).ReturnsAsync(new Category { IsHidden = false });
            _blockedKeywordServiceMock.Setup(r => r.ContainsBlockedKeywordAsync("Blocked title")).ReturnsAsync(true);
            _userKeywordServiceMock.Setup(s => s.GetKeywordsAsync(It.IsAny<int>())).ReturnsAsync(new List<string>());

            var result = await _service.GetNewsByCategoryAndDateRangeAsync("technology", null, null, userId: 1);

            Assert.Empty(result);
        }


        [Fact]
        public async Task GetNewsByCategoryAndDateRangeAsync_ShouldExcludeHiddenArticlesOrTooManyReports()
        {
            var article = NewsArticleDtoMockDataGenerator.GetNewsArticleDto();
            article.IsHidden = true;

            _articleRepoMock.Setup(r => r.GetNewsByCategoryAndDateRangeAsync("technology", null, null)).ReturnsAsync(new List<NewsArticleDto> { article });
            _categoryRepoMock.Setup(r => r.GetCategoryByNameAsync(It.IsAny<string>())).ReturnsAsync(new Category { IsHidden = false });
            _blockedKeywordServiceMock.Setup(r => r.ContainsBlockedKeywordAsync(It.IsAny<string>())).ReturnsAsync(false);
            _reportRepoMock.Setup(r => r.GetReportCountAsync(It.IsAny<int>())).ReturnsAsync(4);
            _userKeywordServiceMock.Setup(s => s.GetKeywordsAsync(It.IsAny<int>())).ReturnsAsync(new List<string>());

            var result = await _service.GetNewsByCategoryAndDateRangeAsync("technology", null, null, userId: 2);

            Assert.Empty(result);
        }


        [Fact]
        public async Task GetNewsByCategoryAndDateRangeAsync_ShouldMapToUserInteractionDto()
        {
            var article = NewsArticleDtoMockDataGenerator.GetNewsArticleDto();
            article.IsHidden = false;

            _articleRepoMock.Setup(r => r.GetNewsByCategoryAndDateRangeAsync("technology", null, null)).ReturnsAsync(new List<NewsArticleDto> { article });
            _categoryRepoMock.Setup(r => r.GetCategoryByNameAsync(article.Category)).ReturnsAsync(new Category { IsHidden = false });
            _blockedKeywordServiceMock.Setup(r => r.ContainsBlockedKeywordAsync(It.IsAny<string>())).ReturnsAsync(false);
            _reportRepoMock.Setup(r => r.GetReportCountAsync(article.NewsArticleId)).ReturnsAsync(0);
            _interactionRepoMock.Setup(r => r.GetInteractionAsync(1, article.NewsArticleId)).ReturnsAsync(new UserArticleInteraction
            {
                IsLiked = true,
                IsDisliked = false
            });
            _userKeywordServiceMock.Setup(s => s.GetKeywordsAsync(It.IsAny<int>())).ReturnsAsync(new List<string>());

            var result = await _service.GetNewsByCategoryAndDateRangeAsync("technology", null, null, 1);

            var dto = Assert.Single(result);
            Assert.True(dto.IsLikedByUser);
            Assert.False(dto.IsDislikedByUser);
        }

    }
}
