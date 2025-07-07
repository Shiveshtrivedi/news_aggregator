using Moq;
using Xunit;
using System.Threading.Tasks;
using news_aggregator.domain.Models;
using news_aggregator.shared.CustomException.NewsArticle;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.tests.Helpers;
using news_application.Models;
using news_aggregator.application.News;

namespace news_aggregator.tests.Services
{
    public class NewsInteractionServiceTest
    {
        private readonly Mock<INewsArticleRepository> _articleRepositoryMock;
        private readonly Mock<IUserArticleInteractionRepository> _interactionRepositoryMock;
        private readonly NewsInteractionService _service;

        public NewsInteractionServiceTest()
        {
            _articleRepositoryMock = new Mock<INewsArticleRepository>();
            _interactionRepositoryMock = new Mock<IUserArticleInteractionRepository>();
            _service = new NewsInteractionService(_articleRepositoryMock.Object, _interactionRepositoryMock.Object);
        }

        [Fact]
        public async Task ToggleLikeAsync_ShouldThrow_WhenArticleDoesNotExist()
        {
            _articleRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((NewsArticle)null!);

            var action = async () => await _service.ToggleLikeAsync(1, 1);

            await Assert.ThrowsAsync<NewsArticleNotFoundException>(action);
        }

        [Fact]
        public async Task ToggleLikeAsync_ShouldAddLike_WhenInteractionDoesNotExist()
        {
            var article = UserArticleInteractionMockDataGenerator.GetNewsArticle();

            _articleRepositoryMock.Setup(repo => repo.GetByIdAsync(article.NewsArticleId)).ReturnsAsync(article);
            _interactionRepositoryMock.Setup(repo => repo.GetInteractionAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((UserArticleInteraction)null!);

            await _service.ToggleLikeAsync(article.NewsArticleId, 101);

            _interactionRepositoryMock.Verify(repo => repo.IncrementLikesAsync(article.NewsArticleId), Times.Once);
        }

        [Fact]
        public async Task ToggleLikeAsync_ShouldCreateInteraction_WhenNoneExists()
        {
            var article = UserArticleInteractionMockDataGenerator.GetNewsArticle();
            int userId = 101;

            _articleRepositoryMock.Setup(repo => repo.GetByIdAsync(article.NewsArticleId)).ReturnsAsync(article);
            _interactionRepositoryMock.Setup(repo => repo.GetInteractionAsync(userId, article.NewsArticleId)).ReturnsAsync((UserArticleInteraction)null!);

            await _service.ToggleLikeAsync(article.NewsArticleId, userId);

            _interactionRepositoryMock.Verify(repo =>
                repo.AddOrUpdateInteractionAsync(It.Is<UserArticleInteraction>(i =>
                    i.UserId == userId &&
                    i.NewsArticleId == article.NewsArticleId &&
                    i.IsLiked == true &&
                    i.IsDisliked == false
                )), Times.Once);
        }

        [Fact]
        public async Task ToggleLikeAsync_ShouldRemoveLike_WhenAlreadyLiked()
        {
            var article = UserArticleInteractionMockDataGenerator.GetNewsArticle();
            var interaction = UserArticleInteractionMockDataGenerator.GetUserArticleInteraction(isLiked: true);

            _articleRepositoryMock.Setup(repo => repo.GetByIdAsync(article.NewsArticleId)).ReturnsAsync(article);
            _interactionRepositoryMock.Setup(repo => repo.GetInteractionAsync(interaction.UserId, interaction.NewsArticleId)).ReturnsAsync(interaction);

            await _service.ToggleLikeAsync(interaction.NewsArticleId, interaction.UserId);

            Assert.False(interaction.IsLiked);
        }

        [Fact]
        public async Task ToggleLikeAsync_ShouldDecrementLike_WhenAlreadyLiked()
        {
            var interaction = UserArticleInteractionMockDataGenerator.GetUserArticleInteraction(isLiked: true);
            var article = UserArticleInteractionMockDataGenerator.GetNewsArticle(interaction.NewsArticleId);

            _articleRepositoryMock.Setup(repo => repo.GetByIdAsync(article.NewsArticleId)).ReturnsAsync(article);
            _interactionRepositoryMock.Setup(repo => repo.GetInteractionAsync(interaction.UserId, article.NewsArticleId)).ReturnsAsync(interaction);

            await _service.ToggleLikeAsync(article.NewsArticleId, interaction.UserId);

            _interactionRepositoryMock.Verify(repo => repo.DecrementLikesAsync(article.NewsArticleId), Times.Once);
        }

        [Fact]
        public async Task ToggleLikeAsync_ShouldSwitchDislikeToLike()
        {
            var interaction = UserArticleInteractionMockDataGenerator.GetUserArticleInteraction(isLiked: false, isDisliked: true);
            var article = UserArticleInteractionMockDataGenerator.GetNewsArticle(interaction.NewsArticleId);

            _articleRepositoryMock.Setup(repo => repo.GetByIdAsync(article.NewsArticleId)).ReturnsAsync(article);
            _interactionRepositoryMock.Setup(repo => repo.GetInteractionAsync(interaction.UserId, article.NewsArticleId)).ReturnsAsync(interaction);

            await _service.ToggleLikeAsync(article.NewsArticleId, interaction.UserId);

            Assert.True(interaction.IsLiked);
            Assert.False(interaction.IsDisliked);
        }

        [Fact]
        public async Task ToggleDislikeAsync_ShouldThrow_WhenArticleDoesNotExist()
        {
            _articleRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((NewsArticle)null!);

            var action = async () => await _service.ToggleDislikeAsync(2, 1);

            await Assert.ThrowsAsync<NewsArticleNotFoundException>(action);
        }

        [Fact]
        public async Task ToggleDislikeAsync_ShouldAddDislike_WhenInteractionDoesNotExist()
        {
            var article = UserArticleInteractionMockDataGenerator.GetNewsArticle();

            _articleRepositoryMock.Setup(repo => repo.GetByIdAsync(article.NewsArticleId)).ReturnsAsync(article);
            _interactionRepositoryMock.Setup(repo => repo.GetInteractionAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((UserArticleInteraction)null!);

            await _service.ToggleDislikeAsync(article.NewsArticleId, 102);

            _interactionRepositoryMock.Verify(repo => repo.IncrementDislikesAsync(article.NewsArticleId), Times.Once);
        }

        [Fact]
        public async Task ToggleDislikeAsync_ShouldRemoveDislike_WhenAlreadyDisliked()
        {
            var interaction = UserArticleInteractionMockDataGenerator.GetUserArticleInteraction(isLiked: false, isDisliked: true);
            var article = UserArticleInteractionMockDataGenerator.GetNewsArticle(interaction.NewsArticleId);

            _articleRepositoryMock.Setup(repo => repo.GetByIdAsync(article.NewsArticleId)).ReturnsAsync(article);
            _interactionRepositoryMock.Setup(repo => repo.GetInteractionAsync(interaction.UserId, article.NewsArticleId)).ReturnsAsync(interaction);

            await _service.ToggleDislikeAsync(article.NewsArticleId, interaction.UserId);

            Assert.False(interaction.IsDisliked);
        }

        [Fact]
        public async Task ToggleDislikeAsync_ShouldDecrementDislike_WhenAlreadyDisliked()
        {
            var interaction = UserArticleInteractionMockDataGenerator.GetUserArticleInteraction(isLiked: false, isDisliked: true);
            var article = UserArticleInteractionMockDataGenerator.GetNewsArticle(interaction.NewsArticleId);

            _articleRepositoryMock.Setup(repo => repo.GetByIdAsync(article.NewsArticleId)).ReturnsAsync(article);
            _interactionRepositoryMock.Setup(repo => repo.GetInteractionAsync(interaction.UserId, article.NewsArticleId)).ReturnsAsync(interaction);

            await _service.ToggleDislikeAsync(article.NewsArticleId, interaction.UserId);

            _interactionRepositoryMock.Verify(repo => repo.DecrementDislikesAsync(article.NewsArticleId), Times.Once);
        }

        [Fact]
        public async Task ToggleDislikeAsync_ShouldSwitchLikeToDislike()
        {
            var interaction = UserArticleInteractionMockDataGenerator.GetUserArticleInteraction(isLiked: true, isDisliked: false);
            var article = UserArticleInteractionMockDataGenerator.GetNewsArticle(interaction.NewsArticleId);

            _articleRepositoryMock.Setup(repo => repo.GetByIdAsync(article.NewsArticleId)).ReturnsAsync(article);
            _interactionRepositoryMock.Setup(repo => repo.GetInteractionAsync(interaction.UserId, article.NewsArticleId)).ReturnsAsync(interaction);

            await _service.ToggleDislikeAsync(article.NewsArticleId, interaction.UserId);

            Assert.True(interaction.IsDisliked);
            Assert.False(interaction.IsLiked);
        }
    }
}
