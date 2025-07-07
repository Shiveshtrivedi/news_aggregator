using news_aggregator.console.Exceptions;
using news_aggregator.console.Http;
using news_aggregator.console.Models;
using news_aggregator.console.Services.Interfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace news_aggregator.console.Services
{
    public class SavedArticleService : ISavedArticleService
    {
        private readonly HttpClient _httpClient;

        public SavedArticleService(IHttpClientFactoryWrapper httpClientFactoryWrapper)
        {
            _httpClient = httpClientFactoryWrapper.GetClient();
        }

        public async Task DeleteArticleAsync(int articleId, int userId)
        {
            var url = $"/api/SavedArticle/{userId}/{articleId}/unsaveArticle";
            try
            {
                var response = await _httpClient.DeleteAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    throw new SavedArticleException($"Failed to delete article {articleId} for user {userId}. Status: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new SavedArticleException("Error occurred while deleting saved article.", ex);
            }
        }

        public async Task<List<NewsArticleDto>> GetSavedArticlesAsync(int userId)
        {
            var url = $"api/SavedArticle/{userId}/getArticleFromUserId";
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    throw new SavedArticleException($"Failed to retrieve saved articles for user {userId}. Status: {response.StatusCode}");
                }

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<NewsArticleDto>>(json) ?? new List<NewsArticleDto>();
            }
            catch (Exception ex)
            {
                throw new SavedArticleException("Error occurred while fetching saved articles.", ex);
            }
        }

        public async Task<bool> SaveArticleAsync(int userId, int articleId)
        {
            var dto = new SaveArticleDto
            {
                UserId = userId,
                ArticleId = articleId
            };

            var url = $"api/SavedArticle/{userId}/{articleId}/saveArticle";

            try
            {
                var response = await _httpClient.PostAsJsonAsync(url, dto);
                if (!response.IsSuccessStatusCode)
                {
                    throw new SavedArticleException($"Failed to save article {articleId} for user {userId}. Status: {response.StatusCode}");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new SavedArticleException("Error occurred while saving article.", ex);
            }
        }
    }

    
}
