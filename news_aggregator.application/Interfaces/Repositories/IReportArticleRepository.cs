using news_aggregator.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application.Interfaces.Repositories
{
    public interface IReportArticleRepository
    {
        Task<bool> HasUserReportedAsync(int userId, int articleId);
        Task AddReportAsync(ReportArticle report);
        Task<int> GetReportCountAsync(int articleId);
    }
}
