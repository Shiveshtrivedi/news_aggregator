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
        public async Task<IEnumerable<NewsArticle>> GetLatestArticlesAsync(string category = "", string keyword = "")
        {
            var apiKey = "aea1338e747c4c57bae18be784ebcd5f";
            var baseUrl = "https://newsapi.org/v2/top-headlines?country=us";

            if (!string.IsNullOrEmpty(category))
                baseUrl += $"&category={category}";
            if (!string.IsNullOrEmpty(keyword))
                baseUrl += $"&q={keyword}";

            baseUrl += $"&apiKey={apiKey}";

            var response = await _httpClient.GetFromJsonAsync<NewsApiResponse>(baseUrl);

            var result = response?.Articles?.Select(a => new NewsArticle
            {
                Title = a.Title ?? "",
                Content = a.Content ?? "",
                PublishedAt = a.PublishedAt,
                Source = a.Source?.Name ?? "Unknown",
                Url = a.Url ?? "",
                Category = ParseCategory(category),
                Likes = 0,
                Dislikes = 0,
            }) ?? new List<NewsArticle>();

            return result;
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
