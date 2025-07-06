using news_aggregator.console.Http;
using news_aggregator.console.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var response = await _httpClient.GetAsync("api/user/keywords");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<string>>() ?? new List<string>();
        }

        public async Task SetKeywordsAsync(int userId, IEnumerable<string> keywords)
        {
            var json = JsonSerializer.Serialize(keywords);

            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"api/user/keywords?userId={userId}", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
