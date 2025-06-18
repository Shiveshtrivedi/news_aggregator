using news_aggregator.console.Http;
using news_aggregator.console.Models;
using news_aggregator.console.Services.Interfaces;
using System.Net.Http.Json;

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
            var response = await _httpClient.DeleteAsync($"/api/SavedArticle/{userId}/{articleId}/unsaveArticle");

            if(!response.IsSuccessStatusCode)
            {
                return;
            }

            return;
        }

        public async Task<List<NewsArticleDto>> GetSavedArticlesAsync(int userId)
        {
            var response = await _httpClient.GetAsync($"api/SavedArticle/{userId}/getArticleFromUserId");

            if (!response.IsSuccessStatusCode)
            {
                return new List<NewsArticleDto>();
            }

            var articles = await response.Content.ReadFromJsonAsync<List<NewsArticleDto>>();
            return articles ?? new List<NewsArticleDto>();
        }

        public async Task<bool> SaveArticleAsync(int userId, int articleId)
        {
            var dto = new SaveArticleDto
            {
                UserId = userId,
                ArticleId = articleId
            };

            var response = await _httpClient.PostAsJsonAsync($"api/SavedArticle/{userId}/{articleId}/saveArticle", dto);

            return response.IsSuccessStatusCode;
        }

    }
}
