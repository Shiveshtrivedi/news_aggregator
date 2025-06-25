using news_aggregator.console.Http;
using news_aggregator.console.Models;
using news_aggregator.console.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;

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
            var response = await _httpClient.GetAsync("api/Category/getAllCategory");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to fetch categories. Status: {response.StatusCode}");
            }

            var categories = await response.Content.ReadFromJsonAsync<List<CategoryDto>>();

            return categories ?? new List<CategoryDto>();
        }

        public async Task<bool> AddCategoryAsync(string categoryName)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Category/addCategory", new { CategoryName = categoryName });
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ToggleCategoryVisibilityAsync(int categoryId)
        {
            var content = new StringContent("", Encoding.UTF8,"application/json");
            var response = await _httpClient.PatchAsync($"api/Category/{categoryId}/categoryVisibilityToggle", content);
            return response.IsSuccessStatusCode;
        }


    }
}
