using AutoMapper;
using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
using news_aggregator.domain.Models.DTOs;
using news_aggregator.shared.CustomException;
using news_aggregator.shared.CustomException.ExternalSource;
using news_aggregator.shared.Validation;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace news_aggregator.application
{
    public class ExternalSourceService : IExternalSourceService
    {
        private readonly IExternalSourceRepository _repository;
        private readonly IMapper _mapper;

        public ExternalSourceService(IExternalSourceRepository repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<ExternalSourceDto>> GetAllSourcesAsync()
        {
            var sources = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ExternalSourceDto>>(sources);
        }

        public async Task<ExternalSourceDto> GetSourceByIdAsync(int externalSourceId)
        {
            var source = await _repository.GetByIdAsync(externalSourceId);
            if (source == null)
                throw new ExternalSourceNotFoundException($"External source with ID {externalSourceId} not found.");

            return _mapper.Map<ExternalSourceDto>(source);
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
            var updated = await _repository.UpdatePartialAsync(id, dto);

            if (!updated)
                throw new ExternalSourceUpdateFailedException($"Update failed for External Source with ID {id}.");

            return true;
        }

    }
}
