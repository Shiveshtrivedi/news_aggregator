using news_aggregator.console.Http;
using news_aggregator.console.Models;
using news_aggregator.console.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Services
{
    public class ServerService : IServerService
    {
        private readonly HttpClient _httpClient;

        public ServerService(IHttpClientFactoryWrapper httpClientFactoryWrapper)
        {
            _httpClient = httpClientFactoryWrapper.GetClient();
        }

        public async Task<ServerDetailsDto> GetServerDetailsAsync(int serverId)
        {
            var response = await _httpClient.GetAsync($"api/ExternalSource/{serverId}/getExternalSoureById");

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<ServerDetailsDto>();
        }

        public async Task<List<ServerStatusDto>> GetServerStatusesAsync()
        {
            var response = await _httpClient.GetAsync("api/ExternalSource/getAllExternalSources");
            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to retrieve statuses");

            return await response.Content.ReadFromJsonAsync<List<ServerStatusDto>>() ?? new();

        }

        public async Task<bool> UpdateServerAsync(int serverId,ServerUpdateDto server)
        {
            var response = await _httpClient.PatchAsJsonAsync($"api/ExternalSource/{serverId}", server);
            return response.IsSuccessStatusCode;
        }
    }
}
