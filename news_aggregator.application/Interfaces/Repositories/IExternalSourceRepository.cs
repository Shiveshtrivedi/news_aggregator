using news_aggregator.domain.Models.DTOs;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application.Interfaces.Repositories
{
    public interface IExternalSourceRepository
    {
        Task<IEnumerable<ExternalSource>> GetAllAsync();
        Task<ExternalSource> GetByIdAsync(int externalServerId);
        Task AddAsync(ExternalSource source);
        Task<bool> UpdateAsync(int id, ExternalSource source);
        Task DeleteAsync(int externalServerId);
        Task<bool> UpdatePartialAsync(int id, UpdateExternalSourceDto dto);
    }
}
