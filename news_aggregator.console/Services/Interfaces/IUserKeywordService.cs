using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Services.Interfaces
{
    public interface IUserKeywordService
    {
        Task<IEnumerable<string>> GetKeywordsAsync();
        Task SetKeywordsAsync(int userId, IEnumerable<string> keywords);
    }
}
