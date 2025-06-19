using news_application.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using news_aggregator.infrastructure.ExternalApi;
using news_aggregator.application.Interfaces.Services;

namespace news_aggregator.infrastructure.Repositories
{
    public class TheNewsApiProvider : INewsProvider
    {
        private readonly HttpClient _httpClient;

        public TheNewsApiProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<NewsArticle>> FetchArticlesAsync(ExternalSource source)
        {
            var response = await _httpClient.GetStringAsync(source.BaseUrl);
            var result = JsonConvert.DeserializeObject<TheNewsApiResponse>(response);

            return result.Data.Select(a => new NewsArticle
            {
                Title = a.Title,
                Description = a.Description,
                Url = a.Url,
                ImageUrl = a.Image_Url,
                PublishedAt = a.Published_At,
                Content = a.Snippet,
                Source = a.Source,
                ExternalSourceId = source.ExternalSourceId
            });
        }
    }
}
