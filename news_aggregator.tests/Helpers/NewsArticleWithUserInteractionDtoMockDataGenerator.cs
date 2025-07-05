using news_aggregator.domain.Models.DTOs;

namespace news_aggregator.tests.Helpers
{
    public static class NewsArticleWithUserInteractionDtoMockDataGenerator
    {
        public static NewsArticleWithUserInteractionDto GetArticleWithUserInteractionDto(
            int articleId = 1,
            bool isLikedByUser = true,
            bool isDislikedByUser = false)
        {
            return new NewsArticleWithUserInteractionDto
            {
                NewsArticleId = articleId,
                Title = $"User Article {articleId}",
                Content = "Article content for user interaction test.",
                Likes = 15,
                Dislikes = 5,
                Category = "Science",
                Source = "Mock User Source",
                Url = $"https://user.mock/article/{articleId}",
                PublishedAt = DateTime.UtcNow.AddHours(-2),
                IsLikedByUser = isLikedByUser,
                IsDislikedByUser = isDislikedByUser
            };
        }

        public static List<NewsArticleWithUserInteractionDto> GetArticleWithUserInteractionDtoList(int count = 3)
        {
            var list = new List<NewsArticleWithUserInteractionDto>();
            for (int i = 1; i <= count; i++)
            {
                list.Add(GetArticleWithUserInteractionDto(articleId: i, isLikedByUser: i % 2 == 0, isDislikedByUser: i % 2 != 0));
            }
            return list;
        }
    }
}
