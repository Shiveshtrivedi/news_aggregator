using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using news_aggregator.application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.infrastructure.Jobs
{
    public class NewsFetcherJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<NewsFetcherJob> _logger;

        public NewsFetcherJob(IServiceProvider serviceProvider, ILogger<NewsFetcherJob> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("NewsFetcherJob is running...");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var newsService = scope.ServiceProvider.GetRequiredService<INewsService>();

                    _logger.LogInformation("Fetching external news...");
                    await newsService.FetchAndSaveExternalNewsAsync();
                    _logger.LogInformation("News fetch completed.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, " Failed to fetch news from external API.");
                }

                await Task.Delay(TimeSpan.FromHours(4), stoppingToken);
            }

        }
      }
}
