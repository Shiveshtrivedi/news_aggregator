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
    public class SearchArticleService : ISearchArticleService
    {
        private readonly HttpClient _httpClient;

        public SearchArticleService(IHttpClientFactoryWrapper clientFactoryWrapper)
        {
            _httpClient = clientFactoryWrapper.GetClient();
        }

        public async Task<List<NewsArticleDto>> SearchArticlesAsync(string query, DateTime? startDate, DateTime? endDate)
        {
            var url = $"api/News/searchNews?title={Uri.EscapeDataString(query)}";

            if (startDate.HasValue)
                url += $"&startDate={startDate.Value:yyyy-MM-dd}";

            if (endDate.HasValue)
                url += $"&endDate={endDate.Value:yyyy-MM-dd}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var articles = await response.Content.ReadFromJsonAsync<List<NewsArticleDto>>();
                return articles ?? new List<NewsArticleDto>();
            }

            return new List<NewsArticleDto>();
        }
    }
}
