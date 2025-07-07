using news_aggregator.infrastructure.Adapter.ExternalApiResponse;
using news_aggregator.infrastructure.Adapter.ExternalNews.Interface;
using news_application.Enum;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace news_aggregator.infrastructure.Adapter.Parsers
{
    public class NewsApiResponseParser : INewsApiResponseParser
    {
        public IEnumerable<NewsArticle> Parse(string rawJson, string sourceName, string category)
        {
            return sourceName switch
            {
                "NewsAPI" => ParseNewsApiResponse(rawJson, category),
                "TheNewsApi" => ParseTheNewsApiResponse(rawJson),
                _ => throw new NotSupportedException($"Unsupported API source: {sourceName}")
            };
        }

        private IEnumerable<NewsArticle> ParseNewsApiResponse(string rawJson, string category)
        {
            var result = JsonSerializer.Deserialize<NewsApiResponse>(rawJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

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
            }) ?? Enumerable.Empty<NewsArticle>();
        }

        private IEnumerable<NewsArticle> ParseTheNewsApiResponse(string rawJson)
        {
            var result = JsonSerializer.Deserialize<TheNewsApiResponse>(rawJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result?.Data?.Select(a => new NewsArticle
            {
                Title = a.Title ?? "",
                Content = a.Snippet ?? "No content available.",
                PublishedAt = a.Published_At,
                Source = a.Source ?? "Unknown",
                Url = a.Url ?? "",
                Category = ParseCategory(a.Categories?.FirstOrDefault() ?? ""),
                Likes = 0,
                Dislikes = 0,
            }) ?? Enumerable.Empty<NewsArticle>();
        }

        private CategoryType ParseCategory(string category)
        {
            return Enum.TryParse<CategoryType>(category, true, out var parsed)
                ? parsed
                : CategoryType.uncategorized;
        }
    }

}
