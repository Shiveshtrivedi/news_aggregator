using news_aggregator.console.Http;
using news_aggregator.console.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace news_aggregator.console.Services
{
    public class BlockedKeywordService : IBlockedKeywordService
    {
        private readonly HttpClient _httpClient;

        public BlockedKeywordService(IHttpClientFactoryWrapper clientFactoryWrapper)
        {
            _httpClient = clientFactoryWrapper.GetClient();
        }

        public async Task<List<string>> GetAllBlockedKeywordsAsync()
        {
            var response = await _httpClient.GetAsync("api/admin/keywords/listBlockedKeyword");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<string>>(json);
        }

        public async Task AddBlockedKeywordAsync(string keyword)
        {
            var content = new StringContent(JsonSerializer.Serialize(keyword), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/admin/keywords/addBlockedKeyword", content);
            response.EnsureSuccessStatusCode();
        }

        public async Task RemoveBlockedKeywordAsync(string keyword)
        {
            var response = await _httpClient.DeleteAsync($"api/admin/keywords/removeBlockedKeyword?keyword={keyword}");
            response.EnsureSuccessStatusCode();
        }
    }
}
