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
    public class BlockedKeywordRepository : IBlockedKeywordRepository
    {
        private readonly NewsDataContext _context;
        public BlockedKeywordRepository(NewsDataContext context) 
        {
            _context = context;
        }
        public async Task AddKeywordAsync(string keyword)
        {
            var existingKeyword = await _context.BlockedKeywords.FirstOrDefaultAsync(keywords=>keywords.Keyword.ToLower()==keyword.ToLower());

            if (existingKeyword != null)
            {
                await _context.BlockedKeywords.AddAsync(new BlockedKeyword {Keyword = keyword });
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task<List<string>> GetAllKeywordsAsync()
        {
           return await _context.BlockedKeywords.Select(keywords => keywords.Keyword).ToListAsync();
        }

        public async Task RemoveKeywordAsync(string keyword)
        {
            var existingKeyword = await _context.BlockedKeywords.FirstOrDefaultAsync(keywords => keywords.Keyword.ToLower() == keyword.ToLower());

            if (existingKeyword != null)
            {
                _context.BlockedKeywords.Remove(existingKeyword);
                await _context.SaveChangesAsync();
            }
        }
    }
}
