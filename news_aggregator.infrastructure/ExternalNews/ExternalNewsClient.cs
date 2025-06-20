using Microsoft.EntityFrameworkCore;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.domain.Models;
using news_aggregator.infrastructure.ExternalApi;
using news_aggregator.infrastructure.ExternalNews.Interface;
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

namespace news_aggregator.infrastructure.ExternalNews
{
    public class ExternalNewsClient : IExternalNewsClient
    {
        private readonly HttpClient _httpClient;
        private readonly INewsRequestBuilder _requestBuilder;
        private readonly INewsApiResponseParser _newsApiResponseParser;


        public ExternalNewsClient(HttpClient httpClient, INewsRequestBuilder requestBuilder, INewsApiResponseParser newsApiResponseParser)
        {
            _httpClient = httpClient;
            _requestBuilder = requestBuilder;
            _newsApiResponseParser = newsApiResponseParser;
        }

        public async Task<IEnumerable<NewsArticle>> GetLatestArticlesAsync(ExternalSource source, string category = "", string keyword = "")
        {
            try
            {

                if (source.ExternalSourceName == "NewsAPI" && string.IsNullOrWhiteSpace(category))
                {
                    var categories = new[] { "technology" };
                    //"sports", "general", "health", "science", "technology""business", "entertainment"};

                    var allArticles = new List<NewsArticle>();

                    foreach (var cat in categories)
                    {
                        var innerArticles = await GetLatestArticlesAsync(source, cat, keyword);
                        allArticles.AddRange(innerArticles);
                    }

                    return allArticles;
                }

                var request = _requestBuilder.BuildRequest(source, category, keyword);
                var response = await _httpClient.SendAsync(request);

                response.EnsureSuccessStatusCode();

                var rawJson = await response.Content.ReadAsStringAsync();
                return _newsApiResponseParser.Parse(rawJson, source.ExternalSourceName, category);
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<NewsArticle>();
            }
        }



        private IEnumerable<NewsArticle> ParseNewsApiResponse(string rawJson, string category)
        {
            var result = JsonSerializer.Deserialize<NewsApiResponse>(rawJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            string debugJson = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });

            Console.WriteLine("altResponse object: ");
            Console.WriteLine(debugJson);

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

        private CategoryType ParseCategory(string category)
        {
            if (Enum.TryParse<CategoryType>(category, true, out var parsedCategory))
                return parsedCategory;

            return CategoryType.uncategorized;
        }
    }
}
