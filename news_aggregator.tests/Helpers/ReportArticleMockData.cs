using news_aggregator.domain.Models.DTOs;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.tests.Helpers
{
    public class ReportArticleMockData
    {
    public static NewsArticle GetTestNewsArticle() => new()
    {
        NewsArticleId = 1,
        Title = "Fake News",
        Content = "Content",
        Url = "http://example.com",
        Source = "TestSource",
        Category = news_application.Enum.CategoryType.general,
        PublishedAt = DateTime.UtcNow,
        ReportCount = 0,
        IsHidden = false
    };

    public static NewsArticleDto GetTestNewsArticleDto() => new()
    {
        NewsArticleId = 1,
        Title = "Fake News",
        Content = "Content",
        Url = "http://example.com",
        Source = "TestSource",
        Category = "general",
        PublishedAt = DateTime.UtcNow
    };

    public static User GetTestUser() => new()
    {
        UserId = 1,
        UserName = "john_doe",
        Email = "john@example.com"
    };
}
}