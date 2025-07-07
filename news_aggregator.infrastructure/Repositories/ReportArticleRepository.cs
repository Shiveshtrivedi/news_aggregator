using Microsoft.EntityFrameworkCore;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.domain.Models;
using news_application.Context;
using news_aggregator.shared.CustomException;

namespace news_aggregator.infrastructure.Repositories
{
    public class ReportArticleRepository : IReportArticleRepository
    {
        private readonly NewsDataContext _newsDataContext;

        public ReportArticleRepository(NewsDataContext newsDataContext)
        {
            _newsDataContext = newsDataContext;
        }

        public async Task AddReportAsync(ReportArticle report)
        {
            try
            {
                _newsDataContext.ReportArticles.Add(report);
                await _newsDataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ReportAddException("Error occurred while adding report.", ex);
            }
        }

        public async Task<int> GetReportCountAsync(int articleId)
        {
            try
            {
                return await _newsDataContext.ReportArticles.CountAsync(report => report.NewsArticleId == articleId);
            }
            catch (Exception ex)
            {
                throw new ReportCountFetchException("Error occurred while fetching report count.", ex);
            }
        }

        public async Task<bool> HasUserReportedAsync(int userId, int articleId)
        {
            try
            {
                return await _newsDataContext.ReportArticles
                    .AnyAsync(report => report.UserId == userId && report.NewsArticleId == articleId);
            }
            catch (Exception ex)
            {
                throw new UserReportCheckException("Error occurred while checking if user has reported.", ex);
            }
        }
    }
}
