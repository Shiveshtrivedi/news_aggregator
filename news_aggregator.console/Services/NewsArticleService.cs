using news_aggregator.console.Http;
using news_aggregator.console.Models;
using news_aggregator.console.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly HttpClient _httpClient;

        public NewsArticleService(IHttpClientFactoryWrapper httpClientFactoryWrapper)
        {
            _httpClient = httpClientFactoryWrapper.GetClient();
        }

        public async Task<List<NewsArticleDto>> GetNewsByCategoryAndDateRangeAsync(string category, DateTime start, DateTime end)
        {
            string url = $"api/News/getNewsByCategoryAndDateRange?category={category}&startDate={start:yyyy-MM-dd}&endDate={end:yyyy-MM-dd}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to fetch articles: {response.StatusCode}");

            var articles = await response.Content.ReadFromJsonAsync<List<NewsArticleDto>>();
            return articles ?? new List<NewsArticleDto>();
        }


        public Task SaveArticleAsync(string articleId)
        {
            throw new NotImplementedException();
        }
    }
}
