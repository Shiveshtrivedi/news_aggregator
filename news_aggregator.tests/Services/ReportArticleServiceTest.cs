using Moq;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.application;
using news_aggregator.shared.CustomException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using news_aggregator.domain.Models;
using news_aggregator.tests.Helpers;
using news_aggregator.domain.Models.DTOs;
using news_application.Models;

namespace news_aggregator.tests.Services
{
    public class ReportArticleServiceTest
    {
        private readonly Mock<IReportArticleRepository> _reportRepoMock;
        private readonly Mock<INewsArticleRepository> _articleRepoMock;
        private readonly Mock<INotificationService> _notificationServiceMock;
        private readonly Mock<INotificationHtmlBuilder> _htmlBuilderMock;
        private readonly Mock<IUserRepository> _userRepoMock;

        private readonly ReportArticleService _service;

        public ReportArticleServiceTest()
        {
            _reportRepoMock = new Mock<IReportArticleRepository>();
            _articleRepoMock = new Mock<INewsArticleRepository>();
            _notificationServiceMock = new Mock<INotificationService>();
            _htmlBuilderMock = new Mock<INotificationHtmlBuilder>();
            _userRepoMock = new Mock<IUserRepository>();

            _service = new ReportArticleService(
                _reportRepoMock.Object,
                _articleRepoMock.Object,
                _notificationServiceMock.Object,
                _htmlBuilderMock.Object,
                _userRepoMock.Object
            );
        }

        [Fact]
        public async Task ReportArticleAsync_ShouldReturnFalse_IfAlreadyReported()
        {
            _reportRepoMock.Setup(r => r.HasUserReportedAsync(1, 1)).ReturnsAsync(true);

            var result = await _service.ReportArticleAsync(1, 1, "test message");

            Assert.False(result);
            _reportRepoMock.Verify(r => r.AddReportAsync(It.IsAny<ReportArticle>()), Times.Never);
        }

        [Fact]
        public async Task ReportArticleAsync_ShouldHideArticle_IfReportsExceedThreshold()
        {
            var article = ReportArticleMockData.GetTestNewsArticle();
            var user = ReportArticleMockData.GetTestUser();
            var articleDto = ReportArticleMockData.GetTestNewsArticleDto();
            var html = "<p>Notification</p>";

            _reportRepoMock.Setup(r => r.HasUserReportedAsync(1, 1)).ReturnsAsync(false);
            _reportRepoMock.Setup(r => r.GetReportCountAsync(1)).ReturnsAsync(3);
            _articleRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(article);
            _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
            _htmlBuilderMock.Setup(h => h.BuildReportNotification(It.IsAny<NewsArticleDto>(), It.IsAny<string>(), It.IsAny<string>())).Returns(html);

            var result = await _service.ReportArticleAsync(1, 1, "inappropriate");

            Assert.True(result);
            Assert.True(article.IsHidden);

            _articleRepoMock.Verify(a => a.UpdateAsync(It.Is<NewsArticle>(x => x.IsHidden && x.ReportCount == 3)), Times.Once);
            _notificationServiceMock.Verify(n => n.NotifyAdminAsync(html), Times.Once);
        }

        [Fact]
        public async Task ReportArticleAsync_ShouldThrowReportProcessException_OnException()
        {
            _reportRepoMock.Setup(r => r.HasUserReportedAsync(1, 1)).ThrowsAsync(new Exception("DB failure"));

            await Assert.ThrowsAsync<ReportProcessException>(() => _service.ReportArticleAsync(1, 1, "fail"));
        }
    }
}
