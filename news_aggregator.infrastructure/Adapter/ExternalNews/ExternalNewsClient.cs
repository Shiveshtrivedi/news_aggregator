using Microsoft.EntityFrameworkCore;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.domain.Models;
using news_aggregator.domain.Models.DTOs;
using news_aggregator.infrastructure.Adapter.ExternalNews.Interface;
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

namespace news_aggregator.infrastructure.Adapter.ExternalNews
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

        public async Task<IEnumerable<NewsArticle>> GetLatestArticlesAsync(ExternalSourceDto source, string category = "", string keyword = "")
        {
            try
            {

                if (source.ExternalSourceName == "NewsAPI" && string.IsNullOrWhiteSpace(category))
                {
                    var categories = new[] { "technology", "sports", "general", "health", "science", "technology","business", "entertainment" };


                    var allArticles = new List<NewsArticle>();

                    foreach (var categoryValue in categories)
                    {
                        var innerArticles = await GetLatestArticlesAsync(source, categoryValue, keyword);
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
            catch (Exception)
            {
                return Enumerable.Empty<NewsArticle>();
            }
        }
    }
}
