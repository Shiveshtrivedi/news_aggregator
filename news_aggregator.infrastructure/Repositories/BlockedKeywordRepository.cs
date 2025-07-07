using Microsoft.EntityFrameworkCore;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.domain.Models;
using news_aggregator.shared.CustomException;
using news_application.Context;
using System;
using System.Collections.Generic;
using System.Linq;
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
            try
            {
                var existingKeyword = await _context.BlockedKeywords
                    .FirstOrDefaultAsync(k => k.Keyword.ToLower() == keyword.ToLower());

                if (existingKeyword == null)
                {
                    await _context.BlockedKeywords.AddAsync(new BlockedKeyword { Keyword = keyword });
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                throw new BlockedKeywordOperationException($"Database error while adding keyword '{keyword}'.", ex);
            }
        }

        public async Task<List<string>> GetAllKeywordsAsync()
        {
            try
            {
                return await _context.BlockedKeywords
                    .Select(k => k.Keyword)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new BlockedKeywordOperationException("Failed to retrieve blocked keywords from database.", ex);
            }
        }

        public async Task RemoveKeywordAsync(string keyword)
        {
            try
            {
                var existingKeyword = await _context.BlockedKeywords
                    .FirstOrDefaultAsync(k => k.Keyword.ToLower() == keyword.ToLower());

                if (existingKeyword != null)
                {
                    _context.BlockedKeywords.Remove(existingKeyword);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException ex)
            {
                throw new BlockedKeywordOperationException($"Failed to remove keyword '{keyword}' from database.", ex);
            }
        }
    }
}
