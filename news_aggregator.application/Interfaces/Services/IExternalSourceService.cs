using news_aggregator.domain.Models.DTOs;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application.Interfaces.Services
{
    public interface IExternalSourceService
    {
        Task<IEnumerable<ExternalSourceDto>> GetAllSourcesAsync();
        Task<ExternalSourceDto> GetSourceByIdAsync(int id);
        Task AddExternalSourceApi(CreateExternalSourceDto dto);
        Task<bool> UpdatePartialAsync(int id, UpdateExternalSourceDto dto);

    }
}
