using AutoMapper;
using Moq;
using news_aggregator.application;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.domain.Models.DTOs;
using news_aggregator.shared.CustomException;
using news_aggregator.tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using news_aggregator.shared.CustomException;

namespace news_aggregator.tests.Services
{
    public class SavedArticleServiceTest
    {
        private readonly Mock<ISavedArticleRepository> _repositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly SavedArticleService _service;

        public SavedArticleServiceTest()
        {
            _repositoryMock = new Mock<ISavedArticleRepository>();
            _mapperMock = new Mock<IMapper>();
            _service = new SavedArticleService(_repositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task SaveArticleAsync_ShouldCallRepository_WhenValidInput()
        {
            await _service.SaveArticleAsync(1, 100);

            _repositoryMock.Verify(repo => repo.SaveArticleAsync(1, 100), Times.Once);
        }

        [Fact]
        public async Task SaveArticleAsync_ShouldThrow_WhenRepositoryFails()
        {
            _repositoryMock
                .Setup(repo => repo.SaveArticleAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new SaveArticleFailedException("Save failed", new Exception("inner")));


            await Assert.ThrowsAsync<SaveArticleFailedException>(() =>
                _service.SaveArticleAsync(1, 100));
        }

        [Fact]
        public async Task GetSavedArticlesByUserIdAsync_ShouldReturnMappedDtos()
        {
            var mockArticles = SavedArticleMockData.GetMockNewsArticles();
            var expectedDtos = SavedArticleMockData.GetMockNewsArticleDtos();

            _repositoryMock.Setup(r => r.GetSavedArticlesByUserIdAsync(1)).ReturnsAsync(mockArticles);
            _mapperMock.Setup(m => m.Map<IEnumerable<NewsArticleDto>>(mockArticles)).Returns(expectedDtos);

            var result = await _service.GetSavedArticlesByUserIdAsync(1);

            Assert.Equal(expectedDtos.Count, result is List<NewsArticleDto> list ? list.Count : 0);
        }

        [Fact]
        public async Task GetSavedArticlesByUserIdAsync_ShouldThrow_WhenRepositoryFails()
        {
            _repositoryMock
                .Setup(repo => repo.GetSavedArticlesByUserIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new GetSavedArticlesFailedException("Failed to get saved articles", new Exception()));

            await Assert.ThrowsAsync<GetSavedArticlesFailedException>(() =>
                _service.GetSavedArticlesByUserIdAsync(1));
        }

        [Fact]
        public async Task DeleteSavedArticleAsync_ShouldCallRepository_WhenValidInput()
        {
            await _service.DeleteSavedArticleAsync(1, 200);

            _repositoryMock.Verify(repo => repo.DeleteSavedArticleAsync(1, 200), Times.Once);
        }

        [Fact]
        public async Task DeleteSavedArticleAsync_ShouldThrow_WhenRepositoryFails()
        {
            _repositoryMock
                .Setup(repo => repo.DeleteSavedArticleAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new DeleteSavedArticleFailedException("Delete failed", new Exception("inner")));


            await Assert.ThrowsAsync<DeleteSavedArticleFailedException>(() =>
                _service.DeleteSavedArticleAsync(1, 200));
        }
    }
}
