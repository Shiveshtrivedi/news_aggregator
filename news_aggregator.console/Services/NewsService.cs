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
    public class NewsService : INewsService
    {
        private readonly HttpClient _httpClient;

        public NewsService(IHttpClientFactoryWrapper clientFactoryWrapper)
        {
            _httpClient = clientFactoryWrapper.GetClient();
        }

        public async Task<List<NewsArticleDto>> GetArticlesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            string url = $"api/News/getNewsByDateRange?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"API call failed: {response.StatusCode}");

            var articles = await response.Content.ReadFromJsonAsync<List<NewsArticleDto>>();

            return articles ?? new List<NewsArticleDto>();
        }

        public async Task<List<NewsArticleDto>> GetNewsByCategoryAndDateRangeAsync(string category, DateTime startDate, DateTime endDate)
        {
            var url = $"api/News/getNewsByCategoryAndDateRange?category={Uri.EscapeDataString(category)}&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return new List<NewsArticleDto>();
            }

            var articles = await response.Content.ReadFromJsonAsync<List<NewsArticleDto>>();
            return articles ?? new List<NewsArticleDto>();

        }

    }
}
