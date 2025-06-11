using Microsoft.EntityFrameworkCore;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.domain.Models;
using news_application.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.infrastructure.Repositories
{
    public class UserKeywordRepository : IUserKeywordRepository
    {
        private readonly NewsDataContext _context;

        public UserKeywordRepository(NewsDataContext newsDataContext)
        {
            _context = newsDataContext;
        }

        public async Task<IEnumerable<UserKeyword>> GetByUserAsync(int userId)
        {
           return  await _context.UserKeywords.Where(u => u.UserId == userId).ToListAsync();
        }
        public async Task AddKeywordsAsync(int userId, IEnumerable<string> keywords)
        {
            var list = keywords.Select(k => new UserKeyword { UserId = userId, Keyword = k });
            await _context.UserKeywords.AddRangeAsync(list);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveAllKeywordsAsync(int userId)
        {
            var existing = _context.UserKeywords.Where(u => u.UserId == userId);
            _context.UserKeywords.RemoveRange(existing);
            await _context.SaveChangesAsync();
        }

    }
}
