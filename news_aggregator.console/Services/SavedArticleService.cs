using news_aggregator.console.Models;
using news_aggregator.console.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Services
{
    public class SavedArticleService : ISavedArticleService
    {
        private readonly HttpClient _httpClient;

        public SavedArticleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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

        public Task<List<NewsArticleDto>> GetSavedArticlesAsync(string userName)
        {
            throw new NotImplementedException();
        }
    }
}
