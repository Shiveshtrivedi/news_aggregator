using news_aggregator.application;
using news_aggregator.infrastructure.ExternalApi;
using news_application.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.infrastructure.Repositories
{
    public class NewsApiProvider : INewsProvider
    {
        private readonly HttpClient _httpClient;

        public NewsApiProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<NewsArticle>> FetchArticlesAsync(ExternalSource source)
        {
            //var response = await _httpClient.GetStringAsync(source.BaseUrl);
            //var result = JsonConvert.DeserializeObject<NewsApiResponse>(response);

            //return result.Articles.Select(a => new NewsArticle
            //{
            //    Title = a.Title,
            //    Description = a.Description,
            //    Url = a.Url,
            //    ImageUrl = a.UrlToImage,
            //    PublishedAt = a.PublishedAt,
            //    Content = a.Content,
            //    Source = a.Source?.Name,
            //    ExternalSourceId = source.ExternalSourceId
            //});
            var response = await _httpClient.GetFromJsonAsync<NewsApiResponse>(source.BaseUrl);

            return response?.Articles.Select(a => new NewsArticle
            {
                Title = a.Title,
                Content = string.IsNullOrWhiteSpace(a.Content) ? a.Description : a.Content,
                Url = a.Url,
                ImageUrl = a.UrlToImage,
                PublishedAt = a.PublishedAt,
                Source = a.Source?.Name ?? source.ExternalSourceName,
                Description = a.Description
            }) ?? new List<NewsArticle>();
        }
    }
}
