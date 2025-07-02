using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Services.Interfaces
{
    public interface IBlockedKeywordService
    {
        Task<List<string>> GetAllBlockedKeywordsAsync();
        Task AddBlockedKeywordAsync(string keyword);
        Task RemoveBlockedKeywordAsync(string keyword);
    }
}
