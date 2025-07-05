using news_aggregator.domain.Models.DTOs;

namespace news_aggregator.tests.Helpers
{
    public static class NewsArticleDtoMockDataGenerator
    {
        public static NewsArticleDto GetNewsArticleDto(
            int articleId = 1,
            string category = "Technology",
            int likes = 10,
            int dislikes = 2,
            bool isHidden = false)
        {
            return new NewsArticleDto
            {
                NewsArticleId = articleId,
                Title = $"Sample Title {articleId}",
                Content = "Sample content for the article.",
                PublishedAt = DateTime.UtcNow.AddDays(-1),
                ExternalSourceId = null,
                Category = category,
                Source = "Mock Source",
                Url = $"https://news.mock/article/{articleId}",
                Likes = likes,
                DisLikes = dislikes,
                IsHidden = isHidden,
                ReportCount = 0
            };
        }

        public static List<NewsArticleDto> GetNewsArticleDtoList(int count = 3)
        {
            var list = new List<NewsArticleDto>();
            for (int i = 1; i <= count; i++)
            {
                list.Add(GetNewsArticleDto(articleId: i));
            }
            return list;
        }
    }
}
