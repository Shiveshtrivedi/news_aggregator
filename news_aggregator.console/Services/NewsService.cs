using news_aggregator.console.DTOs;
using news_aggregator.console.Exceptions;
using news_aggregator.console.Http;
using news_aggregator.console.Models;
using news_aggregator.console.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
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
            var url = $"api/News/getNewsByDateRange?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";
            return await GetArticlesAsync(url, "fetch articles by date range");
        }

        public async Task<List<NewsArticleDto>> GetNewsByCategoryAndDateRangeAsync(string category, DateTime startDate, DateTime endDate)
        {
            var url = $"api/News/getNewsByCategoryAndDateRange?category={Uri.EscapeDataString(category)}&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";
            return await GetArticlesAsync(url, "fetch articles by category and date range");
        }

        public async Task<bool> LikeArticleAsync(int articleId)
        {
            return await SendPostRequestAsync($"api/News/{articleId}/like", null, $"like article {articleId}");
        }

        public async Task<bool> DislikeArticleAsync(int articleId)
        {
            return await SendPostRequestAsync($"api/News/{articleId}/dislike", null, $"dislike article {articleId}");
        }

        public async Task<bool> ReportArticleAsync(int articleId, string message)
        {
            var requestBody = new ReportArticleRequestDto
            {
                ArticleId = articleId,
                Message = message
            };

            return await SendPostRequestAsync($"api/News/{articleId}/report", requestBody, $"report article {articleId}");
        }

        private async Task<List<NewsArticleDto>> GetArticlesAsync(string url, string context)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    throw new NewsServiceException($"Failed to {context}. Status code: {response.StatusCode}");

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<NewsArticleDto>>(json) ?? new List<NewsArticleDto>();
            }
            catch (Exception ex)
            {
                throw new NewsServiceException($"Error while attempting to {context}.", ex);
            }
        }

        private async Task<bool> SendPostRequestAsync(string url, object? body, string context)
        {
            try
            {
                HttpResponseMessage response;
                if (body is null)
                {
                    response = await _httpClient.PostAsync(url, null);
                }
                else
                {
                    response = await _httpClient.PostAsJsonAsync(url, body);
                }

                if (!response.IsSuccessStatusCode)
                    throw new NewsServiceException($"Failed to {context}. Status code: {response.StatusCode}");

                return true;
            }
            catch (Exception ex)
            {
                throw new NewsServiceException($"Error while attempting to {context}.", ex);
            }
        }
    }
}
