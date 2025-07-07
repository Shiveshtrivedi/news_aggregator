using news_aggregator.console.Exceptions;
using news_aggregator.console.Http;
using news_aggregator.console.Models;
using news_aggregator.console.Services.Interfaces;
using System;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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

        public async Task<UserDto> LoginAsync(string email, string password)
        {
            var loginRequest = new LoginRequestDto(email, password);

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);

                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException("Login failed. Please check your credentials.");

                var user = await response.Content.ReadFromJsonAsync<UserDto>();

                HandleLoginSuccess(user);

                return user!;
            }
            catch (Exception ex)
            {
                throw new AuthServiceException("An error occurred during login.", ex);
            }
        }

        public async Task<bool> SignUpAsync(UserDto userDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/signup", userDto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new AuthServiceException("An error occurred during sign-up.", ex);
            }
        }

        private void HandleLoginSuccess(UserDto? user)
        {
            if (user == null)
                throw new InvalidOperationException("Login succeeded but user data is missing.");

            Session.SetUser(user.UserId, user.UserName, user.Token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", user.Token);
        }
    }

    public record LoginRequestDto(string Email, string Password);

}
