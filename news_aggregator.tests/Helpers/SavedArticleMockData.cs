using news_aggregator.domain.Models.DTOs;
using news_application.Enum;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.tests.Helpers
{
    public static class SavedArticleMockData
    {
        public static List<NewsArticle> GetMockNewsArticles() =>
            new()
            {
                new NewsArticle
                {
                    NewsArticleId = 1,
                    Title = "Test Article 1",
                    Content = "Test Content 1",
                    PublishedAt = DateTime.UtcNow,
                    Url = "https://example.com/1",
                    Source = "UnitTestSource",
                    Category = CategoryType.general
                },
                new NewsArticle
                {
                    NewsArticleId = 2,
                    Title = "Test Article 2",
                    Content = "Test Content 2",
                    PublishedAt = DateTime.UtcNow,
                    Url = "https://example.com/2",
                    Source = "UnitTestSource",
                    Category = CategoryType.business
                }
            };

        public static List<NewsArticleDto> GetMockNewsArticleDtos() =>
            new()
            {
                new NewsArticleDto
                {
                    NewsArticleId = 1,
                    Title = "Test Article 1",
                    Content = "Test Content 1",
                    PublishedAt = DateTime.UtcNow,
                    Url = "https://example.com/1",
                    Source = "UnitTestSource",
                    Category = "general"
                },
                new NewsArticleDto
                {
                    NewsArticleId = 2,
                    Title = "Test Article 2",
                    Content = "Test Content 2",
                    PublishedAt = DateTime.UtcNow,
                    Url = "https://example.com/2",
                    Source = "UnitTestSource",
                    Category = "business"
                }
            };
    }
}
