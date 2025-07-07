using news_aggregator.console.DTOs;
using news_aggregator.console.Exceptions;
using news_aggregator.console.Http;
using news_aggregator.console.Models;
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
    public class NotificationService : INotificationService
    {
        private readonly HttpClient _httpClient;

        public NotificationService(IHttpClientFactoryWrapper httpClientFactoryWrapper)
        {
            _httpClient = httpClientFactoryWrapper.GetClient();
        }

        public async Task<List<string>> GetNotificationsAsync(int userId)
        {
            try
            {
                var url = $"api/NotificationConfig/{userId}";
                return await GetFromApiAsync<List<string>>(url, $"fetch notifications for user {userId}") ?? new List<string>();
            }
            catch(Exception)
            {
                throw new Exception();
            }
        }

        public async Task<NotificationConfigDto> GetConfigAsync(int userId)
        {
            try
            {
                var url = $"api/NotificationConfig/{userId}";
                return await GetFromApiAsync<NotificationConfigDto>(url, $"fetch notification config for user {userId}") ?? new NotificationConfigDto();
            }
            catch(Exception)
            {
                throw new Exception();
            }            
        }

        public async Task ToggleCategoryAsync(int userId, string category, bool enable)
        {
            try
            {
                var url = $"api/NotificationConfig/toggle?userId={userId}&category={category}&enable={enable}";
                await PostToApiAsync(url, null, $"toggle category '{category}' for user {userId}");
            }
            catch(Exception)
            {
                throw new Exception();
            }            
        }

        public async Task SubmitKeywordsAsync(int userId, string keywords)
        {
            try
            {
                var keywordDto = new UserKeywordDto { Keywords = keywords };
                var url = $"api/NotificationConfig/keywords?userId={userId}";
                await PostToApiAsync(url, keywordDto, $"submit keywords for user {userId}");
            }
            catch(Exception)
            {
                throw new Exception();
            }
            
        }

        private async Task<T?> GetFromApiAsync<T>(string url, string context)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    throw new NotificationServiceException($"Failed to {context}. Status: {response.StatusCode}");

                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                throw new NotificationServiceException($"Error while trying to {context}.", ex);
            }
        }

        private async Task PostToApiAsync(string url, object? payload, string context)
        {
            try
            {
                HttpResponseMessage response;
                if (payload == null)
                    response = await _httpClient.PostAsync(url, null);
                else
                    response = await _httpClient.PostAsJsonAsync(url, payload);

                if (!response.IsSuccessStatusCode)
                    throw new NotificationServiceException($"Failed to {context}. Status: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                throw new NotificationServiceException($"Error while trying to {context}.", ex);
            }
        }
    }

}
