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
        Task<IEnumerable<ExternalSource>> GetAllSourcesAsync();
        Task<ExternalSource> GetSourceByIdAsync(int id);
        Task AddSourceAsync(ExternalSource source);
        Task UpdateSourceAsync(ExternalSource source);
        Task DeleteSourceAsync(int externalSourceId);

    }
}
