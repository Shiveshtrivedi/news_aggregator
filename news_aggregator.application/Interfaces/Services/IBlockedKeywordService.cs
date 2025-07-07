using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application.Interfaces.Services
{
    public interface IBlockedKeywordService
    {
        Task AddKeywordAsync(string keyword);
        Task<List<string>> GetAllKeywordsAsync();
        Task RemoveKeywordAsync(string keyword);
        Task<bool> ContainsBlockedKeywordAsync(string text);
    }

}
