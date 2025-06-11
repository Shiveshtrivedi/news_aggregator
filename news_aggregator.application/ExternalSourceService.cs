using news_aggregator.application.Interfaces.Repositories;
using news_aggregator.application.Interfaces.Services;
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

        public async Task AddSourceAsync(ExternalSource source)
        {
            await _repository.AddAsync(source);
        }

        public async Task DeleteSourceAsync(int externalSourceId)
        {
            await _repository.DeleteAsync(externalSourceId);
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

        public async Task UpdateSourceAsync(ExternalSource source)
        {
            await _repository.UpdateAsync(source);
        }
    }
}
