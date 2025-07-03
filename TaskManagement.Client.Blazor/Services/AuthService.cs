using Blazored.LocalStorage;
using System.Net.Http.Json;
using TaskManagement.Client.Blazor.Models.Auth;

namespace TaskManagement.Client.Blazor.Services
{
    public class AuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILocalStorageService _localStorage;

        public AuthService(IHttpClientFactory httpClientFactory, ILocalStorageService localStorage)
        {
            _httpClientFactory = httpClientFactory;
            _localStorage = localStorage;
        }

        public async Task<bool> LoginAsync(LoginModel model)
        {
            var client = _httpClientFactory.CreateClient("AuthorizedAPI");

            var response = await client.PostAsJsonAsync("api/Auth/login", model);
            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                await _localStorage.SetItemAsync("authToken", token);
                return true;
            }

            return false;
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");
        }
    }
}
