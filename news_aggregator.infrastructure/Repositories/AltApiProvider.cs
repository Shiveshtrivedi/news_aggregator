using news_aggregator.application;
using news_application.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using news_aggregator.infrastructure.ExternalApi;

namespace news_aggregator.infrastructure.Repositories
{
    public class AltApiProvider : INewsProvider
    {
        private readonly HttpClient _httpClient;

        public AltApiProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<NewsArticle>> FetchArticlesAsync(ExternalSource source)
        {
            var response = await _httpClient.GetStringAsync(source.BaseUrl);
            var result = JsonConvert.DeserializeObject<AltNewsApiResponse>(response);

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
