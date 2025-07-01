using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application
{
    public class BlockedKeywordService : IBlockedKeywordService
    {
        private readonly IBlockedKeywordRepository _blockedKeywordRepository;

        public BlockedKeywordService(IBlockedKeywordRepository blockedKeywordRepository)
        {
            _blockedKeywordRepository = blockedKeywordRepository;
        }

        public Task AddKeywordAsync(string keyword)
        {
           return _blockedKeywordRepository.AddKeywordAsync(keyword);
        }

        public Task<List<string>> GetAllKeywordsAsync()
        {
            return _blockedKeywordRepository.GetAllKeywordsAsync();
        }
        public Task RemoveKeywordAsync(string keyword)
        {
            return _blockedKeywordRepository.RemoveKeywordAsync(keyword);
        }

        public async Task<bool> ContainsBlockedKeywordAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;

            var keywords = await _blockedKeywordRepository.GetAllKeywordsAsync();
            return keywords.Any(keyword => text.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }
    }
}
