using news_aggregator.console.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Services.Interfaces
{
    public interface IServerService
    {
        Task<List<ServerStatusDto>> GetServerStatusesAsync();
        Task<ServerDetailsDto?> GetServerDetailsAsync(int serverId);
        Task<bool> UpdateServerAsync(int serverId,ServerUpdateDto server);
    }

}
