using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.domain.Models.DTOs;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application
{
    public class ExternalSourceService : IExternalSourceService
    {
        private readonly IExternalSourceRepository _repository;

        public ExternalSourceService(IExternalSourceRepository repository)
        {
            _repository = repository;
        }


        public Task<IEnumerable<ExternalSource>> GetAllSourcesAsync()
        {
            var sources = _repository.GetAllAsync();
            return sources;
        }

        public Task<ExternalSource> GetSourceByIdAsync(int externalSourceId)
        {
            var source = _repository.GetByIdAsync(externalSourceId);
            return source;
        }

        public async Task AddExternalSourceApi(CreateExternalSourceDto dto)
        {
            var source = new ExternalSource
            {
                ExternalSourceName = dto.ExternalSourceName,
                ApiKey = dto.ApiKey,
                BaseUrl = dto.BaseUrl,
                IsActive = dto.IsActive,
                LastAccessed = DateTime.UtcNow
            };

            await _repository.AddAsync(source);
        }

        public async Task<bool> UpdatePartialAsync(int id, UpdateExternalSourceDto dto)
        {
            return await _repository.UpdatePartialAsync(id, dto);
        }

    }
}
