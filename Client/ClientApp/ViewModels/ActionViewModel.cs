using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ClientApp.ViewModels;

public partial class ActionViewModel : ObservableObject
{
    [ObservableProperty]
    private int coachId;

    [ObservableProperty]
    private int classId;

    [ObservableProperty]
    private string clientName;

    [ObservableProperty]
    private bool status = true;

    private const string BaseUrl = "https://localhost:7123";

    [RelayCommand]
    private async Task CreateActionAsync()
    {
        var token = Preferences.Get("jwt_token", null);
        if (string.IsNullOrEmpty(token))
        {
            await Application.Current.MainPage.DisplayAlert(
                "Помилка", "Немає токена. Увійдіть заново.", "OK");
            return;
        }

        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var payload = new
        {
            coachId = CoachId,
            classId = ClassId,
            clientName = ClientName,
            status = Status
        };

        var content = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json");

        var response = await client.PostAsync($"{BaseUrl}/api/actions", content);

        if (response.IsSuccessStatusCode)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Успіх", "Бронювання створено", "OK");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert(
                "Помилка", "Не вдалося створити бронювання", "OK");
        }
    }
}
