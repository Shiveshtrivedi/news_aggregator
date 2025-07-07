using news_aggregator.console.DTOs;
using news_aggregator.console.Http;
using news_aggregator.console.Models;
using news_aggregator.console.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
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
            try
            {
                var response = await _httpClient.GetAsync("api/admin/keywords/listBlockedKeyword");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to retrieve blocked keywords.", ex);
            }
        }

        public async Task AddBlockedKeywordAsync(string keyword)
        {
            try
            {
                var payload = new BlockedKeywordDto { Keyword = keyword };
                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/admin/keywords/addBlockedKeyword", content);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to add blocked keyword: {keyword}", ex);
            }
        }

        public async Task RemoveBlockedKeywordAsync(string keyword)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/admin/keywords/removeBlockedKeyword?keyword={keyword}");
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to remove blocked keyword: {keyword}", ex);
            }
        }
    }
}
