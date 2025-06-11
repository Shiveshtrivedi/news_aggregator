using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using news_aggregator.domain.Models;
using news_application.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using news_application.Models;

namespace news_aggregator.infrastructure.ExternalApi
{
    public class ExternalNewsClient
    {
        private readonly HttpClient _httpClient;
        private readonly NewsApiOptions _options;
        private readonly NewsDataContext _context;

        public ExternalNewsClient(HttpClient httpClient, IOptions<NewsApiOptions> options, NewsDataContext context)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _context = context;
        }

        public async Task<List<NewsArticle>> FetchAndMapArticlesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<NewsApiResponse>(
                $"{_options.BaseUrl}/top-headlines?country=in&apiKey={_options.ApiKey}");

            var externalSource = await _context.ExternalSources.FirstOrDefaultAsync(e => e.ExternalSourceName == "NewsAPI");
            if (externalSource == null) throw new Exception("ExternalSource not found.");

            var defaultCategory = await _context.Categories.FirstOrDefaultAsync();
            if (defaultCategory == null) throw new Exception("No category available.");

            return response?.Articles.Select(a => new NewsArticle
            {
                Title = a.Title,
                Content = a.Description ?? a.Content ?? "",
                PublishedAt = a.PublishedAt,
                Source = a.Source.Name,
                //CategoryId = defaultCategory.CategoryId,
                //ExternalSourceId = externalSource.ExternalSourceId
            }).ToList() ?? new List<NewsArticle>();
        }
    }
}
