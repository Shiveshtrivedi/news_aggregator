using Microsoft.Extensions.DependencyInjection;
using news_aggregator.application.Interfaces.Services;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.infrastructure.Repositories
{
    public class NewsProviderFactory : INewsProviderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public NewsProviderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public INewsProvider GetProvider(string sourceName)
        {
            return sourceName.ToLower() switch
            {
                "newsapi" => _serviceProvider.GetRequiredService<NewsApiProvider>(),
                "conversation" => _serviceProvider.GetRequiredService<AltApiProvider>(),
                _ => throw new NotSupportedException($"No provider configured for source: {sourceName}")
            };
        }
    }

}
