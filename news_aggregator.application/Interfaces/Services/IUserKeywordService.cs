using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application.Interfaces.Services
{
    public interface IUserKeywordService
    {
        Task<IEnumerable<string>> GetKeywordsAsync(int userId);
        Task SetKeywordsAsync(int userId, IEnumerable<string> keywords);
    }
}
