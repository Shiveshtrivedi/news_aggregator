using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application
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
            var keyword = await _userKeywordRepository.GetByUserAsync(userId);

            return keyword.Select(keyword => keyword.Keyword);
        }
        public async Task SetKeywordsAsync(int userId, IEnumerable<string> keywords)
        {
            await _userKeywordRepository.RemoveAllKeywordsAsync(userId);
            if (keywords.Any())
                await _userKeywordRepository.AddKeywordsAsync(userId, keywords);
        }
    }
}

