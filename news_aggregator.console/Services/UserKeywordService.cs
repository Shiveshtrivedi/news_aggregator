using news_aggregator.console.DTOs;
using news_aggregator.console.Exceptions;
using news_aggregator.console.Http;
using news_aggregator.console.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace news_aggregator.console.Services
{
    public class UserKeywordService : IUserKeywordService
    {
        private readonly HttpClient _httpClient;

        public UserKeywordService(IHttpClientFactoryWrapper httpClientFactoryWrapper)
        {
            _httpClient = httpClientFactoryWrapper.GetClient();
        }

        public async Task<IEnumerable<string>> GetKeywordsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/user/keywords");

                if (!response.IsSuccessStatusCode)
                    throw new UserKeywordServiceException($"Failed to fetch keywords. Status: {response.StatusCode}");

                var keywords = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
                return keywords ?? new List<string>();
            }
            catch (Exception ex)
            {
                throw new UserKeywordServiceException("Error while fetching keywords.", ex);
            }
        }

        public async Task SetKeywordsAsync(int userId, IEnumerable<string> keywords)
        {
            var dto = new UserKeywordsDto { Keywords = keywords };

            try
            {
                var content = BuildJsonContent(dto);
                var response = await _httpClient.PostAsync($"api/user/keywords?userId={userId}", content);

                if (!response.IsSuccessStatusCode)
                    throw new UserKeywordServiceException($"Failed to set keywords for user {userId}. Status: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                throw new UserKeywordServiceException($"Error while setting keywords for user {userId}.", ex);
            }
        }

        private static StringContent BuildJsonContent(object data)
        {
            var json = JsonSerializer.Serialize(data);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }

    
}
