using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.tests.Helpers
{
    public static class NotificationMockDataGenerator
    {
        public static Notification GetNotification()
        {
            return new Notification
            {
                NotificationId = 1,
                UserId = 100,
                Message = "Test message",
                SentAt = DateTime.UtcNow,
                IsRead = false
            };
        }

        public static List<Notification> GetNotificationList()
        {
            return new List<Notification>
            {
                new Notification
                {
                    NotificationId = 1,
                    UserId = 100,
                    Message = "First message",
                    SentAt = DateTime.UtcNow.AddMinutes(-10),
                    IsRead = false
                },
                new Notification
                {
                    NotificationId = 2,
                    UserId = 100,
                    Message = "Second message",
                    SentAt = DateTime.UtcNow.AddMinutes(-5),
                    IsRead = true
                }
            };
        }
    }
}
