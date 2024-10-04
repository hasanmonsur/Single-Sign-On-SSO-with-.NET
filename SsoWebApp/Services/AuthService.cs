using SsoWebApp.Contacts;
using SsoWebApp.Models;
using System.Net.Http.Headers;

namespace SsoWebApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AuthService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> RegisterAsync(UserRegistrationModel model, string token)
        {
            // Set the authorization header            
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsJsonAsync("api/Auth/Register", model);
            
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> LoginAsync(UserLoginModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", model);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
