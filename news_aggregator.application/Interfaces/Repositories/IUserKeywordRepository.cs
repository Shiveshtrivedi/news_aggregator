using news_aggregator.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application.Interfaces.Repositories
{
    public interface IUserKeywordRepository
    {
        Task<IEnumerable<UserKeyword>> GetByUserAsync(int userId);
        Task AddKeywordsAsync(int userId, IEnumerable<string> keywords);
        Task RemoveAllKeywordsAsync(int userId);
    }
}
