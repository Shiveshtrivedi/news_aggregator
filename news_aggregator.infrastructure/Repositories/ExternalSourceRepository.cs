using Microsoft.EntityFrameworkCore;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.domain.Models.DTOs;
using news_application.Context;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.infrastructure.Repositories
{
    public class ExternalSourceRepository : GenericRepository<ExternalSource>, IExternalSourceRepository
    {
        private readonly NewsDataContext _context;

        public ExternalSourceRepository(NewsDataContext context) : base(context)
        {
            _context = context;
        }
       
        public async Task<bool> UpdateAsync(int id, ExternalSource source)
        {
            var existing = await _context.ExternalSources.FindAsync(id);
            if (existing == null) return false;

            existing.ExternalSourceName = source.ExternalSourceName;
            existing.ApiKey = source.ApiKey;
            existing.BaseUrl = source.BaseUrl;
            existing.IsActive = source.IsActive;
            existing.LastAccessed = source.LastAccessed;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePartialAsync(int id, UpdateExternalSourceDto dto)
        {
            var existing = await _context.ExternalSources.FindAsync(id);
            if (existing == null) return false;

            if (dto.ExternalSourceName != null) existing.ExternalSourceName = dto.ExternalSourceName;
            if (dto.ApiKey != null) existing.ApiKey = dto.ApiKey;
            if (dto.BaseUrl != null) existing.BaseUrl = dto.BaseUrl;
            if (dto.AuthParamName != null) existing.AuthParamName = dto.AuthParamName;
            if (dto.AuthLocation != null) existing.AuthLocation = dto.AuthLocation;
            if (dto.IsActive.HasValue) existing.IsActive = dto.IsActive.Value;
            if (dto.LastAccessed.HasValue) existing.LastAccessed = dto.LastAccessed.Value;

            await _context.SaveChangesAsync();
            return true;
        }

    }
}
