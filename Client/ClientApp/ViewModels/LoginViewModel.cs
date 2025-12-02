using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text;
using System.Text.Json;

namespace ClientApp.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string password;

    // ⚠️ Підстав свій URL до ServerApp
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

        var client = new HttpClient();

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

        Preferences.Set("jwt_token", token);

        // Перехід на сторінку зі списком Items (Classes)
        await Shell.Current.GoToAsync("//ItemsPage");
    }
}
