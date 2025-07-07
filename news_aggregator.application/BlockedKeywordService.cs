using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.shared.CustomException;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task AddKeywordAsync(string keyword)
        {
            try
            {
                await _blockedKeywordRepository.AddKeywordAsync(keyword);
            }
            catch (BlockedKeywordOperationException ex)
            {
                throw new ApplicationException($"Unable to add keyword '{keyword}'.", ex);
            }
        }

        public async Task<List<string>> GetAllKeywordsAsync()
        {
            try
            {
                return await _blockedKeywordRepository.GetAllKeywordsAsync();
            }
            catch (BlockedKeywordOperationException ex)
            {
                throw new ApplicationException("Failed to load blocked keywords.", ex);
            }
        }

        public async Task RemoveKeywordAsync(string keyword)
        {
            try
            {
                await _blockedKeywordRepository.RemoveKeywordAsync(keyword);
            }
            catch (BlockedKeywordOperationException ex)
            {
                throw new ApplicationException($"Unable to remove keyword '{keyword}'.", ex);
            }
        }

        public async Task<bool> ContainsBlockedKeywordAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;

            var keywords = await GetAllKeywordsAsync();    
            return keywords.Any(keyword => text.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        }
    }
}
