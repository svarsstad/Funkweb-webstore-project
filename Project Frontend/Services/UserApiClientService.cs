// Services/UserApiClient.cs
using Microsoft.AspNetCore.Identity.Data;
using Project_Backend.Models;
using System.Net.Http.Json;

namespace Project_Frontend.Services
{
    public class UserApiClientService
    {
        private readonly HttpClient _http;

        public UserApiClientService(HttpClient http)
        {
            _http = http;
        }
        public async Task<AuthResponse> LoginAsync(string email, string password)
        {
            var request = new Project_Backend.Models.LoginRequest { Email = email, Password = password };
    
            var response = await _http.PostAsJsonAsync("api/User/login", request);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AuthResponse>()
                       ?? new AuthResponse { Success = false, Message = "EMPTY RESPONSE" };
            }

            var errorResult = await response.Content.ReadFromJsonAsync<AuthResponse>();
            return errorResult ?? new AuthResponse { Success = false, Message = "AUTHENTICATION FAILED" };
        }
    }
}