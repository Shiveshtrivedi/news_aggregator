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
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService(IHttpClientFactoryWrapper clientFactoryWrapper)
        {
            _httpClient = clientFactoryWrapper.GetClient();
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Category/getAllCategory");

                EnsureSuccess(response, "fetch categories");

                var categories = await DeserializeResponse<List<CategoryDto>>(response);
                return categories ?? new List<CategoryDto>();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error occurred while retrieving categories.", ex);
            }
        }

        public async Task<bool> AddCategoryAsync(string categoryName)
        {
            try
            {
                var payload = new AddCategoryRequestDto { CategoryName = categoryName };
                var response = await _httpClient.PostAsJsonAsync("api/Category/addCategory", payload);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to add category: {categoryName}", ex);
            }
        }

        public async Task<bool> ToggleCategoryVisibilityAsync(int categoryId)
        {
            try
            {
                var content = new StringContent("", Encoding.UTF8, "application/json");
                var response = await _httpClient.PatchAsync($"api/Category/{categoryId}/categoryVisibilityToggle", content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to toggle visibility for category ID: {categoryId}", ex);
            }
        }

        private static void EnsureSuccess(HttpResponseMessage response, string operation)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to {operation}. Status: {response.StatusCode}");
            }
        }

        private static async Task<T?> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(json, options);
        }

    }


}
