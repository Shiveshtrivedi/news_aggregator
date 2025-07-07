using Moq;
using news_aggregator.application;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.tests.Services
{
    public class UserKeywordServiceTest
    {
        private readonly Mock<IUserKeywordRepository> _repositoryMock;
        private readonly UserKeywordService _service;

        public UserKeywordServiceTest()
        {
            _repositoryMock = new Mock<IUserKeywordRepository>();
            _service = new UserKeywordService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetKeywordsAsync_ShouldReturnKeywordStrings()
        {
            var mockUserKeywords = UserKeywordMockData.GetUserKeywords();

            _repositoryMock
                .Setup(r => r.GetByUserAsync(1))
                .ReturnsAsync(mockUserKeywords);

            var result = await _service.GetKeywordsAsync(1);

            Assert.Equal(mockUserKeywords.Count, result.Count());
            Assert.Contains("AI", result);
        }

        [Fact]
        public async Task GetKeywordsAsync_ShouldThrow_WhenRepositoryFails()
        {
            _repositoryMock
                .Setup(r => r.GetByUserAsync(It.IsAny<int>()))
                .ThrowsAsync(new System.Exception("DB failure"));

            await Assert.ThrowsAsync<System.Exception>(() => _service.GetKeywordsAsync(1));
        }

        [Fact]
        public async Task SetKeywordsAsync_ShouldAddOnlyNewKeywords()
        {
            var existing = new List<string> { "ai", "health" };
            var input = new List<string> { "AI", "sports", "finance" };
            var expectedToAdd = new List<string> { "sports", "finance" };

            _repositoryMock
                .Setup(r => r.GetExistingKeywordsAsync(1))
                .ReturnsAsync(existing);

            _repositoryMock
                .Setup(r => r.AddKeywordsAsync(1, expectedToAdd))
                .Returns(Task.CompletedTask)
                .Verifiable();

            await _service.SetKeywordsAsync(1, input);

            _repositoryMock.Verify();
        }

        [Fact]
        public async Task SetKeywordsAsync_ShouldNotAdd_WhenNoNewKeyword()
        {
            var existing = new List<string> { "ai", "sports" };
            var input = new List<string> { "AI", "Sports" };

            _repositoryMock
                .Setup(r => r.GetExistingKeywordsAsync(1))
                .ReturnsAsync(existing);

            await _service.SetKeywordsAsync(1, input);

            _repositoryMock.Verify(r => r.AddKeywordsAsync(It.IsAny<int>(), It.IsAny<IEnumerable<string>>()), Times.Never);
        }

        [Fact]
        public async Task SetKeywordsAsync_ShouldThrow_WhenRepositoryFails()
        {
            _repositoryMock
                .Setup(r => r.GetExistingKeywordsAsync(It.IsAny<int>()))
                .ThrowsAsync(new System.Exception("DB error"));

            await Assert.ThrowsAsync<System.Exception>(() => _service.SetKeywordsAsync(1, new List<string> { "tech" }));
        }
    }
}
