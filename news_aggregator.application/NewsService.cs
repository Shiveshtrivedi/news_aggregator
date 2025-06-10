using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application
{
    public class NewsService : INewsService
    {
        private readonly IExternalNewsClient _externalNewsClient;
        private readonly INewsArticleRepository _newsArticleRepository;

        public NewsService(IExternalNewsClient externalNewsClient, INewsArticleRepository newsArticleRepository)
        {
            _externalNewsClient = externalNewsClient;
            _newsArticleRepository = newsArticleRepository;
        }

        public async Task<IEnumerable<NewsArticle>> FetchExternalNewsAsync()
        {
            return await _externalNewsClient.GetLatestArticlesAsync();
        }

        public async Task<IEnumerable<NewsArticle>> FetchAndSaveExternalNewsAsync()
        {
            var articles = await _externalNewsClient.GetLatestArticlesAsync();

            foreach (var article in articles)
            {
                if (string.IsNullOrWhiteSpace(article.Content))
                {
                    article.Content = "No content available.";
                }

                bool exists = await _newsArticleRepository.ExistsAsync(article.Title);

                if (!exists)
                {
                    await _newsArticleRepository.AddAsync(article);
                }
            }

            return articles;
        }
    }

}
