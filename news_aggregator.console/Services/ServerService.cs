using news_aggregator.console.Exceptions;
using news_aggregator.console.Http;
using news_aggregator.console.Models;
using news_aggregator.console.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text.Json;
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
            var url = $"api/ExternalSource/{serverId}/getExternalSoureById";

            try
            {
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    throw new ServerServiceException($"Failed to get server details for serverId {serverId}. Status: {response.StatusCode}");

                var data = await response.Content.ReadFromJsonAsync<ServerDetailsDto>();
                if (data == null)
                    throw new ServerServiceException($"No server details returned for serverId {serverId}");

                return data;
            }
            catch (Exception ex)
            {
                throw new ServerServiceException($"Error while getting server details for serverId {serverId}", ex);
            }
        }

        public async Task<List<ServerStatusDto>> GetServerStatusesAsync()
        {
            var url = "api/ExternalSource/getAllExternalSources";

            try
            {
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    throw new ServerServiceException($"Failed to retrieve server statuses. Status: {response.StatusCode}");

                var data = await response.Content.ReadFromJsonAsync<List<ServerStatusDto>>();
                return data ?? new List<ServerStatusDto>();
            }
            catch (Exception ex)
            {
                throw new ServerServiceException("Error while retrieving server statuses.", ex);
            }
        }

        public async Task<bool> UpdateServerAsync(int serverId, ServerUpdateDto server)
        {
            var url = $"api/ExternalSource/{serverId}/updateExternalSource";

            try
            {
                var response = await _httpClient.PatchAsJsonAsync(url, server);
                if (!response.IsSuccessStatusCode)
                    throw new ServerServiceException($"Failed to update server with ID {serverId}. Status: {response.StatusCode}");

                return true;
            }
            catch (Exception ex)
            {
                throw new ServerServiceException($"Error while updating server with ID {serverId}.", ex);
            }
        }
    }

   
}
