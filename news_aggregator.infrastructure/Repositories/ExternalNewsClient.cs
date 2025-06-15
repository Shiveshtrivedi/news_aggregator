using Microsoft.EntityFrameworkCore;
using news_aggregator.application;
using news_aggregator.domain.Models;
using news_aggregator.infrastructure.ExternalApi;
using news_application.Context;
using news_application.Enum;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace news_aggregator.infrastructure.Repositories
{
    public class ExternalNewsClient : IExternalNewsClient
    {
        private readonly HttpClient _httpClient;

        public ExternalNewsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<NewsArticle>> GetLatestArticlesAsync(ExternalSource source, string category = "", string keyword = "")
        {
            var queryParams = new List<string>();

            if (!string.IsNullOrWhiteSpace(category))
                queryParams.Add($"category={Uri.EscapeDataString(category)}");

            if (!string.IsNullOrWhiteSpace(keyword))
                queryParams.Add($"q={Uri.EscapeDataString(keyword)}");

            var url = source.BaseUrl;
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            if (source.AuthLocation == "query" && !string.IsNullOrWhiteSpace(source.AuthParamName))
            {
                queryParams.Add($"{source.AuthParamName}={Uri.EscapeDataString(source.ApiKey)}");
            }

            if (queryParams.Any())
            {
                var separator = url.Contains('?') ? "&" : "?";
                request.RequestUri = new Uri($"{url}{separator}{string.Join("&", queryParams)}");
            }

            if (source.AuthLocation == "header" && !string.IsNullOrWhiteSpace(source.AuthParamName))
            {
                request.Headers.Add(source.AuthParamName, source.ApiKey);
            }

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var rawJson = await response.Content.ReadAsStringAsync();
            Console.WriteLine("RAW RESPONSE:");
            Console.WriteLine(rawJson);

            if (source.ExternalSourceName == "conversation")
            {
                var altResponse = JsonSerializer.Deserialize<AltNewsApiResponse>(rawJson);

                return altResponse?.Data?.Select(a => new NewsArticle
                {
                    Title = a.Title ?? "",
                    Content = a.Snippet ?? "No content available.",
                    PublishedAt = a.Published_At,
                    Source = a.Source ?? "Unknown",
                    Url = a.Url ?? "",
                    Category = ParseCategory(category),
                    Likes = 0,
                    Dislikes = 0,
                }) ?? new List<NewsArticle>();
            }
            else
            {
                var result = await response.Content.ReadFromJsonAsync<NewsApiResponse>();

                return result?.Articles?.Select(a => new NewsArticle
                {
                    Title = a.Title ?? "",
                    Content = a.Content ?? "No content available.",
                    PublishedAt = a.PublishedAt,
                    Source = a.Source?.Name ?? "Unknown",
                    Url = a.Url ?? "",
                    Category = ParseCategory(category),
                    Likes = 0,
                    Dislikes = 0,
                }) ?? new List<NewsArticle>();
            }
        }

        private CategoryType ParseCategory(string category)
        {
            if (Enum.TryParse<CategoryType>(category, true, out var parsedCategory))
            {
                return parsedCategory;
            }
            return CategoryType.Uncategorized;
        }

    }


}
