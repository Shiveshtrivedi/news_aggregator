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
    public class NotificationService : INotificationService
    {
        private readonly HttpClient _httpClient;

        public NotificationService(IHttpClientFactoryWrapper httpClientFactoryWrapper)
        {
            _httpClient = httpClientFactoryWrapper.GetClient();
        }

        public async Task<List<string>> GetNotificationsAsync(int userId)
        {
            var response = await _httpClient.GetAsync($"api/NotificationConfig/{userId}");

            response.EnsureSuccessStatusCode();

            var notifications = await response.Content.ReadFromJsonAsync<List<string>>();
            return notifications ?? new List<string>();
        }

        public async Task<NotificationConfigDto> GetConfigAsync(int userId)
        {
            var response = await _httpClient.GetAsync($"api/NotificationConfig/{userId}");

            response.EnsureSuccessStatusCode();

            var config = await response.Content.ReadFromJsonAsync<NotificationConfigDto>();
            return config ?? new NotificationConfigDto();
        }

        public async Task ToggleCategoryAsync(int userId, string category, bool enable)
        {
            var url = $"api/NotificationConfig/toggle?userId={userId}&category={category}&enable={enable}";
            var response = await _httpClient.PostAsync(url, null);

            response.EnsureSuccessStatusCode();
        }

        public async Task SubmitKeywordsAsync(int userId, string keywords)
        {
            var content = new StringContent($"\"{keywords}\"", Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"api/NotificationConfig/keywords?userId={userId}", content);

            response.EnsureSuccessStatusCode();
        }

    }
}
