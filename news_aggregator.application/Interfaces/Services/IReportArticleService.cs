using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application.Interfaces.Services
{
    public interface IReportArticleService
    {
        Task<bool> ReportArticleAsync(int articleId, int userId, string message);
    }
}
