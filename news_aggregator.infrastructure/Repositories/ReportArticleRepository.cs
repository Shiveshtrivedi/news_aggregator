using Microsoft.EntityFrameworkCore;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.domain.Models;
using news_application.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _newsDataContext.ReportArticles.Add(report);
            await _newsDataContext.SaveChangesAsync();
        }

        public async Task<int> GetReportCountAsync(int articleId)
        {
            return await _newsDataContext.ReportArticles.CountAsync(report=>report.NewsArticleId==articleId);
        }

        public Task<bool> HasUserReportedAsync(int userId, int articleId)
        {
            return _newsDataContext.ReportArticles.AnyAsync(report => report.UserId == userId && report.NewsArticleId==articleId);
        }
    }
}
