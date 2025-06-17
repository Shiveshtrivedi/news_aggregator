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
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(IHttpClientFactoryWrapper clientFactoryWrapper)
        {
            _httpClient = clientFactoryWrapper.GetClient();
        }
        public async Task<UserDto> LoginAsync(string email,string password)
        {
            var loginRequest = new { Email = email, Password = password };

            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);

            if (!response.IsSuccessStatusCode)
                return null;

            var user = await response.Content.ReadFromJsonAsync<UserDto>();
            return user;

        }

        public async Task<bool> SignUpAsync(UserDto userDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/signup", userDto);
            return response.IsSuccessStatusCode;
        }
    }
}
