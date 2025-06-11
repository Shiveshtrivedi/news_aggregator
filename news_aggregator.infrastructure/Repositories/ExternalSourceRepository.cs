using Microsoft.EntityFrameworkCore;
using news_aggregator.application.Interfaces.Repositories;
using news_application.Context;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.infrastructure.Repositories
{
    public class ExternalSourceRepository : IExternalSourceRepository
    {
        private readonly NewsDataContext _context;

        public ExternalSourceRepository(NewsDataContext context)
        {
            _context = context;
        }
        public async Task AddAsync(ExternalSource source)
        {
            _context.ExternalSources.Add(source);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int externalServerId)
        {
            var externalSource = await _context.ExternalSources.FindAsync(externalServerId);

            if (externalSource != null)
            {
                _context.ExternalSources.Remove(externalSource);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ExternalSource>> GetAllAsync()
        {
            var externalSources = await _context.ExternalSources.ToListAsync();

            return externalSources;
        }

        public async Task<ExternalSource> GetByIdAsync(int externalServerId)
        {
            var externalSource = await _context.ExternalSources.FindAsync(externalServerId);

            return externalSource;
        }

        public async Task UpdateAsync(ExternalSource source)
        {
            _context.ExternalSources.Update(source);
            await _context.SaveChangesAsync();
        }
    }
}
