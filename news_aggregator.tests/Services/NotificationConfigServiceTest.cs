using Moq;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.shared.CustomException.CategoryException;
using news_aggregator.tests.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using news_application.Models;
using news_aggregator.domain.Models.DTOs;
using news_aggregator.application.Notification;

namespace news_aggregator.tests.Services
{
    public class NotificationConfigServiceTest
    {
        private readonly Mock<INotificationConfigRepository> _configRepoMock = new();
        private readonly Mock<ICategoryRepository> _categoryRepoMock = new();
        private readonly Mock<INotificationService> _notificationServiceMock = new();
        private readonly Mock<INewsQueryService> _newsQueryServiceMock = new();
        private readonly Mock<INotificationHtmlBuilder> _htmlBuilderMock = new();
        private readonly NotificationConfigService _service;

        public NotificationConfigServiceTest()
        {
            _service = new NotificationConfigService(
                _configRepoMock.Object,
                _categoryRepoMock.Object,
                _notificationServiceMock.Object,
                _newsQueryServiceMock.Object,
                _htmlBuilderMock.Object
            );
        }

        [Fact]
        public async Task GetOrCreateForUserAsync_ReturnsConfig()
        {
            var expected = NotificationConfigMockDataGenerator.GetNotificationConfig();
            _configRepoMock.Setup(r => r.GetOrCreateAsync(1)).ReturnsAsync(expected);

            var result = await _service.GetOrCreateForUserAsync(1);

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task UpdateConfigAsync_CallsRepo()
        {
            var config = NotificationConfigMockDataGenerator.GetNotificationConfig();

            await _service.UpdateConfigAsync(config);

            _configRepoMock.Verify(r => r.AddOrUpdateAsync(config), Times.Once);
        }

        [Fact]
        public async Task ToggleCategoryAsync_EnablesCategoryAndSendsNotification()
        {
            var config = NotificationConfigMockDataGenerator.GetNotificationConfig();
            _configRepoMock.Setup(r => r.GetOrCreateAsync(1)).ReturnsAsync(config);
            _categoryRepoMock.Setup(r => r.CategoryExistsAsync("business")).ReturnsAsync(true);
            _newsQueryServiceMock.Setup(r => r.GetNewsByCategoryAsync("business"))
                                 .ReturnsAsync(NotificationConfigMockDataGenerator.GetNewsArticles());
            _htmlBuilderMock.Setup(b => b.Build("business", It.IsAny<IEnumerable<NewsArticleDto>>()))
                            .Returns("HTML content");

            await _service.ToggleCategoryAsync(1, "business", true);

            _configRepoMock.Verify(r => r.AddOrUpdateAsync(config), Times.Once);
            _notificationServiceMock.Verify(n => n.NotifyUserAsync(1, "HTML content"), Times.Once);
        }

        [Fact]
        public async Task ToggleCategoryAsync_InvalidCategory_ThrowsException()
        {
            var config = NotificationConfigMockDataGenerator.GetNotificationConfig();
            _configRepoMock.Setup(r => r.GetOrCreateAsync(1)).ReturnsAsync(config);
            _categoryRepoMock.Setup(r => r.CategoryExistsAsync("invalid")).ReturnsAsync(false);

            await Assert.ThrowsAsync<CategoryNotFoundException>(() =>
                _service.ToggleCategoryAsync(1, "invalid", true));
        }

        [Fact]
        public async Task ToggleCategoryAsync_KeywordCategory_TogglesFlag()
        {
            var config = NotificationConfigMockDataGenerator.GetNotificationConfig();
            _configRepoMock.Setup(r => r.GetOrCreateAsync(1)).ReturnsAsync(config);

            await _service.ToggleCategoryAsync(1, "keywords", true);

            Assert.True(config.KeywordsEnabled);
            _configRepoMock.Verify(r => r.AddOrUpdateAsync(config), Times.Once);
        }
    }
}
