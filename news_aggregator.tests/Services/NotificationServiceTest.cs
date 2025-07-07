using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using news_aggregator.application;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using news_application.Models;

namespace news_aggregator.tests.Services
{
    public class NotificationServiceTest
    {
        private readonly Mock<INotificationRepository> _notificationRepoMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<IConfiguration> _configMock;
        private readonly NotificationService _service;

        public NotificationServiceTest()
        {
            _notificationRepoMock = new Mock<INotificationRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _configMock = new Mock<IConfiguration>();

            _configMock.Setup(c => c["Smtp:Host"]).Returns("smtp.test.com");
            _configMock.Setup(c => c["Smtp:Port"]).Returns("587");
            _configMock.Setup(c => c["Smtp:User"]).Returns("testuser");
            _configMock.Setup(c => c["Smtp:Password"]).Returns("testpass");
            _configMock.Setup(c => c["Smtp:Email"]).Returns("noreply@test.com");
            _configMock.Setup(c => c["Admin:UserId"]).Returns("1");
            _configMock.Setup(c => c["Admin:Email"]).Returns("admin@test.com");

            _service = new NotificationService(
                _notificationRepoMock.Object,
                _configMock.Object,
                _userRepoMock.Object
            );
        }

        [Fact]
        public async Task CreateNotificationAsync_ShouldCallRepoWithCorrectData()
        {
            await _service.CreateNotificationAsync(1, "Hello User");

            _notificationRepoMock.Verify(repo =>
                repo.AddNotificationAsync(It.Is<Notification>(n =>
                    n.UserId == 1 &&
                    n.Message == "Hello User" &&
                    n.SentAt <= DateTime.UtcNow
                )),
                Times.Once);
        }

        [Fact]
        public async Task GetUserNotificationsAsync_ShouldReturnUserNotifications()
        {
            var expectedNotifications = new List<Notification>
            {
                new Notification { NotificationId = 1, UserId = 1, Message = "Test", SentAt = DateTime.UtcNow }
            };

            _notificationRepoMock.Setup(r => r.GetUserNotificationsAsync(1)).ReturnsAsync(expectedNotifications);

            var result = await _service.GetUserNotificationsAsync(1);

            Assert.Single(result);
            Assert.Equal("Test", ((List<Notification>)result)[0].Message);
        }

        [Fact]
        public async Task NotifyUserAsync_ShouldSendNotificationAndEmail()
        {
            var user = new User
            {
                UserId = 2,
                Email = "user@test.com"
            };

            _userRepoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(user);

            await _service.NotifyUserAsync(2, "You got notified!");

            _notificationRepoMock.Verify(r => r.AddNotificationAsync(It.IsAny<Notification>()), Times.Once);
        }

        [Fact]
        public async Task NotifyUserAsync_ShouldThrowException_WhenUserNotFound()
        {
            _userRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((User)null!);

            await Assert.ThrowsAsync<Exception>(() => _service.NotifyUserAsync(5, "Missing user"));
        }

        [Fact]
        public async Task NotifyAdminAsync_ShouldSendAdminNotification()
        {
            await _service.NotifyAdminAsync("<b>Important</b>");

            _notificationRepoMock.Verify(r => r.AddNotificationAsync(It.Is<Notification>(n =>
                n.UserId == 1 &&
                n.Message.Contains("reported", StringComparison.OrdinalIgnoreCase)
            )), Times.Once);
        }
    }
}
