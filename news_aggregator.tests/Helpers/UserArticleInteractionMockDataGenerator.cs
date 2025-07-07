using news_aggregator.domain.Models;
using news_application.Enum;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.tests.Helpers
{
    public static class UserArticleInteractionMockDataGenerator
    {
        public static UserArticleInteraction GetUserArticleInteraction(
           int interactionId = 1,
           int userId = 101,
           int articleId = 501,
           bool isLiked = true,
           bool isDisliked = false,
           DateTime? timestamp = null)
        {
            return new UserArticleInteraction
            {
                UserArticleInteractionId = interactionId,
                UserId = userId,
                NewsArticleId = articleId,
                IsLiked = isLiked,
                IsDisliked = isDisliked,
                Timestamp = timestamp ?? DateTime.UtcNow,
                User = GetUser(userId),
                NewsArticle = GetNewsArticle(articleId)
            };
        }

        public static NewsArticle GetNewsArticle(int articleId = 501)
        {
            return new NewsArticle
            {
                NewsArticleId = articleId,
                Title = $"Test Article Title {articleId}",
                Content = "This is a sample news article content used for testing.",
                Source = "Mock News Source",
                Description = "Mock description of the article.",
                ImageUrl = "https://example.com/image.jpg",
                Url = $"https://news.example.com/article/{articleId}",
                PublishedAt = DateTime.UtcNow.AddDays(-1),
                Category = CategoryType.technology,
                Likes = 10,
                Dislikes = 2,
                ExternalSourceId = null,
                IsHidden = false,
                ReportCount = 0,
                Reports = new List<ReportArticle>()
            };
        }

        public static User GetUser(int userId = 101)
        {
            return new User
            {
                UserId = userId,
                UserName = $"TestUser{userId}",
                Email = $"testuser{userId}@example.com",
                Password = "hashed_password",
                Role = UserRole.Admin,
                IsTokenActive = true,
                RefreshToken = "mock_refresh_token",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7)
            };
        }

        public static List<UserArticleInteraction> GetUserArticleInteractionList(int count = 3)
        {
            var list = new List<UserArticleInteraction>();
            for (int i = 1; i <= count; i++)
            {
                list.Add(GetUserArticleInteraction(
                    interactionId: i,
                    userId: 100 + i,
                    articleId: 200 + i,
                    isLiked: i % 2 == 0,
                    isDisliked: i % 2 != 0
                ));
            }
            return list;
        }
    }
}
