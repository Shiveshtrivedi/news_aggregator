using news_aggregator.console.Exceptions;
using news_aggregator.console.Http;
using news_aggregator.console.Models;
using news_aggregator.console.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text.Json;
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
            try
            {
                var url = BuildSearchUrl(query, startDate, endDate);
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new SearchArticleException($"Search request failed with status code {response.StatusCode}");
                }

                return await DeserializeArticles(response) ?? new List<NewsArticleDto>();
            }
            catch (Exception ex)
            {
                throw new SearchArticleException("An error occurred while searching for articles.", ex);
            }
        }

        private static string BuildSearchUrl(string query, DateTime? startDate, DateTime? endDate)
        {
            var url = $"api/News/searchNews?title={Uri.EscapeDataString(query)}";

            if (startDate.HasValue)
                url += $"&startDate={startDate.Value:yyyy-MM-dd}";

            if (endDate.HasValue)
                url += $"&endDate={endDate.Value:yyyy-MM-dd}";

            return url;
        }

        private static async Task<List<NewsArticleDto>?> DeserializeArticles(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<NewsArticleDto>>(json);
        }
    }
}
