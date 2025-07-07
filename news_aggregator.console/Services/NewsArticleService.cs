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
    public class NewsArticleService : INewsArticleService
    {
        private readonly HttpClient _httpClient;

        public NewsArticleService(IHttpClientFactoryWrapper httpClientFactoryWrapper)
        {
            _httpClient = httpClientFactoryWrapper.GetClient();
        }

        public async Task<List<NewsArticleDto>> GetNewsByCategoryAndDateRangeAsync(string category, DateTime start, DateTime end)
        {
            try
            {
                var url = BuildUrl(category, start, end);
                var response = await _httpClient.GetAsync(url);

                EnsureSuccessStatus(response);

                var articles = await DeserializeArticles(response);
                return articles ?? new List<NewsArticleDto>();
            }
            catch (Exception ex)
            {
                throw new NewsArticleFetchException("An error occurred while fetching news articles.", ex);
            }
        }

        private static string BuildUrl(string category, DateTime start, DateTime end)
        {
            return $"api/News/getNewsByCategoryAndDateRange?category={category}&startDate={start:yyyy-MM-dd}&endDate={end:yyyy-MM-dd}";
        }

        private static void EnsureSuccessStatus(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new NewsArticleFetchException($"Failed to fetch articles. Status code: {response.StatusCode}");
            }
        }

        private static async Task<List<NewsArticleDto>?> DeserializeArticles(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<NewsArticleDto>>(json);
        }
    }

   
}
