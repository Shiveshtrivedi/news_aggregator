using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace news_aggregator.application
{
    public class NewsService : INewsService
    {
        private readonly IExternalNewsClient _externalNewsClient;
        private readonly INewsArticleRepository _newsArticleRepository;
        private readonly IExternalSourceRepository _externalSourceRepository;
        private readonly INewsProviderFactory _newsProviderFactory;

        public NewsService(
            IExternalNewsClient externalNewsClient,
            INewsArticleRepository newsArticleRepository,
            IExternalSourceRepository externalSourceRepository,
            INewsProviderFactory newsProviderFactory)
        {
            _externalNewsClient = externalNewsClient;
            _newsArticleRepository = newsArticleRepository;
            _externalSourceRepository = externalSourceRepository;
            _newsProviderFactory = newsProviderFactory;
        }

        public async Task<IEnumerable<NewsArticle>> FetchAndSaveExternalNewsAsync()
        {
            var allArticles = new List<NewsArticle>();
            var sources = await _externalSourceRepository.GetAllAsync();
            var activeSources = sources.Where(s => s.IsActive).ToList();

            foreach (var source in activeSources)
            {
                try
                {
                    var provider = _newsProviderFactory.GetProvider(source.ExternalSourceName);
                    var articles = await _externalNewsClient.GetLatestArticlesAsync(source);

                    foreach (var article in articles)
                    {
                        if (string.IsNullOrWhiteSpace(article.Content))
                        {
                            article.Content = "No content available.";
                        }

                        article.ExternalSourceId = source.ExternalSourceId;

                        bool exists = await _newsArticleRepository.ExistsAsync(article.Title, article.Url);

                        if (!exists)
                        {
                            await _newsArticleRepository.AddAsync(article);
                        }
                    }

                    source.LastAccessed = DateTime.UtcNow;
                    await _externalSourceRepository.UpdateAsync(source.ExternalSourceId, source);

                    allArticles.AddRange(articles);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to fetch from source {source.ExternalSourceName}: {ex.Message}");
                }
            }

            return allArticles;
        }
    }
}
