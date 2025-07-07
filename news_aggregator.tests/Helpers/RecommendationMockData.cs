using news_application.Enum;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.tests.Helpers
{
    public static class RecommendationMockData
    {
        public static NewsArticle GetArticle(int id, string title, string content, CategoryType category = CategoryType.general)
        {
            return new NewsArticle
            {
                NewsArticleId = id,
                Title = title,
                Content = content,
                Category = category,
                PublishedAt = DateTime.UtcNow,
                Url = $"http://test.com/{id}",
                Source = "Test Source",
                Likes = 0,
                Dislikes = 0,
                IsHidden = false,
                ReportCount = 0
            };
        }

        public static List<NewsArticle> GetAllArticles() => new()
{
    GetArticle(1, "AI and future", "AI is evolving rapidly", CategoryType.technology),
    GetArticle(2, "Climate impact", "Climate change is alarming", CategoryType.uncategorized),
    GetArticle(3, "Sports update", "Football world cup news", CategoryType.sports),
};


        public static List<NewsArticle> GetSavedArticles() => new()
        {
            GetArticle(1, "AI and future", "AI is evolving rapidly", CategoryType.technology),
            GetArticle(2, "Climate impact", "Climate change is alarming", CategoryType.uncategorized)
        };

        public static List<int> GetLikedArticleIds() => new() { 1 };

        public static List<string> GetUserKeywords() => new() { "climate", "AI" };
    }
}
