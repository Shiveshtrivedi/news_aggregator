using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace news_aggregator.application.Keyword
{
    public class UserKeywordService : IUserKeywordService
    {
        private readonly IUserKeywordRepository _userKeywordRepository;

        public UserKeywordService(IUserKeywordRepository userKeywordRepository)
        {
            _userKeywordRepository = userKeywordRepository;
        }

        public async Task<IEnumerable<string>> GetKeywordsAsync(int userId)
        {
            try
            {
                var keyword = await _userKeywordRepository.GetByUserAsync(userId);
                return keyword.Select(keyword => keyword.Keyword);
            }
            catch
            {
                throw;
            }
        }

        public async Task SetKeywordsAsync(int userId, IEnumerable<string> keywords)
        {
            try
            {
                var existingKeywords = await _userKeywordRepository.GetExistingKeywordsAsync(userId);

                var newKeywords = keywords
                    .Where(k => !existingKeywords.Contains(k, StringComparer.OrdinalIgnoreCase))
                    .ToList();

                if (newKeywords.Any())
                {
                    await _userKeywordRepository.AddKeywordsAsync(userId, newKeywords);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
