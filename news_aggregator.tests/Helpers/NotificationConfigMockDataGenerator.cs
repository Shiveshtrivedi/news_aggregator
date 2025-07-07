using news_aggregator.domain.Models.DTOs;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.tests.Helpers
{
    public static class NotificationConfigMockDataGenerator
    {
        public static NotificationConfig GetNotificationConfig(int userId = 1)
        {
            return new NotificationConfig
            {
                UserId = userId,
                KeywordsEnabled = false,
                CategorySettings = new List<NotificationCategorySetting>
                {
                    new NotificationCategorySetting
                    {
                        UserId = userId,
                        CategoryName = "business",
                        IsEnabled = false
                    }
                }
            };
        }

        public static IEnumerable<NewsArticleDto> GetNewsArticles()
        {
            return new List<NewsArticleDto>
            {
                new () { Title = "Article 1", Content = "Content 1" },
                new () { Title = "Article 2", Content = "Content 2" }
            };
        }
    }
}
