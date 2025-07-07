using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.domain.Models;
using news_aggregator.domain.Models.DTOs;
using news_aggregator.shared.CustomException;

namespace news_aggregator.application
{
    public class ReportArticleService : IReportArticleService
    {
        private readonly IReportArticleRepository _reportReposiotry;
        private readonly INewsArticleRepository _articleRepository;
        private readonly INotificationService _notificationService;
        private readonly INotificationHtmlBuilder _notificationHtmlBuilder;
        private readonly IUserRepository _userRepository;

        private const int ReportThreshold = 3;

        public ReportArticleService(
            IReportArticleRepository reportReposiotry,
            INewsArticleRepository articleRepository,
            INotificationService notificationService,
            INotificationHtmlBuilder notificationHtmlBuilder,
            IUserRepository userRepository)
        {
            _reportReposiotry = reportReposiotry;
            _articleRepository = articleRepository;
            _notificationService = notificationService;
            _notificationHtmlBuilder = notificationHtmlBuilder;
            _userRepository = userRepository;
        }

        public async Task<bool> ReportArticleAsync(int articleId, int userId, string message)
        {
            try
            {
                var alreadyReported = await _reportReposiotry.HasUserReportedAsync(userId, articleId);
                if (alreadyReported) return false;

                var report = new ReportArticle
                {
                    NewsArticleId = articleId,
                    UserId = userId,
                    Message = message
                };

                await _reportReposiotry.AddReportAsync(report);

                var count = await _reportReposiotry.GetReportCountAsync(articleId);
                var article = await _articleRepository.GetByIdAsync(articleId);

                var articleDto = new NewsArticleDto
                {
                    NewsArticleId = article.NewsArticleId,
                    Title = article.Title,
                    Content = article.Content,
                    Url = article.Url,
                    Source = article.Source,
                    Category = article.Category.ToString(),
                    PublishedAt = article.PublishedAt
                };

                if (article != null)
                {
                    article.ReportCount = count;
                    article.IsHidden = count >= ReportThreshold;
                    await _articleRepository.UpdateAsync(article);
                }

                var reportingUser = await _userRepository.GetByIdAsync(userId);
                var notificationHtml = _notificationHtmlBuilder.BuildReportNotification(articleDto, message, reportingUser.UserName);

                await _notificationService.NotifyAdminAsync(notificationHtml);
                return true;
            }
            catch (Exception ex)
            {
                throw new ReportProcessException("An error occurred while processing the report.", ex);
            }
        }
    }
}
