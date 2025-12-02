using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Maui.Storage;
using ClientApp.Models;

namespace ClientApp.Services
{
    public class ApiService
    {
        private const string BaseUrl = "https://localhost:7123";

        private readonly HttpClient _http;

        public ApiService()
        {
            _http = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };

            TryAttachJwt();
        }

        // Додає токен у Authorization, якщо існує в Preferences
        private void TryAttachJwt()
        {
            var token = Preferences.Default.Get("jwt_token", null as string);

            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // ---------- LOGIN ----------
        public async Task<string?> LoginAsync(string email, string password)
        {
            var body = new { email, password };

            var response = await _http.PostAsJsonAsync("/api/auth/login", body);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return json?.Token;
        }

        private class LoginResponse
        {
            public string Token { get; set; } = "";
        }

        // ---------- GET ITEMS ----------
        public async Task<List<ItemModel>?> GetItemsAsync()
        {
            TryAttachJwt(); // оновити заголовок на випадок нового токена

            var response = await _http.GetAsync("/api/items");

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<List<ItemModel>>();
        }

        // ---------- CREATE ACTION ----------
        public async Task<bool> CreateActionAsync(int coachId, int classId, string clientName, bool status)
        {
            TryAttachJwt(); // оновлення токена перед запитом

            var body = new
            {
                coachId,
                classId,
                clientName,
                status
            };

            var response = await _http.PostAsJsonAsync("/api/actions", body);

            return response.IsSuccessStatusCode;
        }
    }
}
