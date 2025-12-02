using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace ClientApp.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string password;

    // ⚠️ Постав свій реальний URL (порт з ServerApp)
    private const string BaseUrl = "https://localhost:7123";

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            await Application.Current.MainPage.DisplayAlert(
                "Помилка", "Введіть email та пароль", "OK");
            return;
        }

        try
        {
            using var client = new HttpClient();

            var payload = new
            {
                email = Email,
                password = Password
            };

            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync($"{BaseUrl}/api/auth/login", content);

            if (!response.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Помилка", "Невірний email або пароль", "OK");
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var token = doc.RootElement.GetProperty("token").GetString();

            if (string.IsNullOrEmpty(token))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Помилка", "Сервер не повернув токен", "OK");
                return;
            }

            // зберігаємо токен
            Preferences.Default.Set("jwt_token", token);

            // перехід на сторінку списку
            await Shell.Current.GoToAsync("//ItemsPage");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            await Application.Current.MainPage.DisplayAlert(
                "Помилка", "Сталася помилка під час логіну.", "OK");
        }
    }
}
