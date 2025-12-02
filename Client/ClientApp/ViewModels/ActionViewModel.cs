using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;
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

    [ObservableProperty]
    private bool isBusy;

    [ObservableProperty]
    private string message;

    private const string BaseUrl = "https://localhost:7123";

    [RelayCommand]
    private async Task CreateActionAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        Message = string.Empty;

        try
        {
            // 1. Дістаємо токен
            var token = Preferences.Default.Get("jwt_token", null as string);
            if (string.IsNullOrEmpty(token))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Помилка", "Немає токена. Увійдіть заново.", "OK");
                await Shell.Current.GoToAsync("//LoginPage");
                return;
            }

            // 2. Формуємо HTTP-клієнт
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            // 3. Формуємо тіло запиту
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

            // 4. POST /api/actions
            var response = await client.PostAsync($"{BaseUrl}/api/actions", content);

            if (response.IsSuccessStatusCode)
            {
                Message = "Бронювання створено.";
                await Application.Current.MainPage.DisplayAlert(
                    "Успіх", "Бронювання створено", "OK");
                // за бажанням можна очистити форму:
                // ClientName = string.Empty;
            }
            else
            {
                Message = "Не вдалося створити бронювання.";
                await Application.Current.MainPage.DisplayAlert(
                    "Помилка", "Не вдалося створити бронювання", "OK");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            Message = "Сталася помилка під час створення.";
            await Application.Current.MainPage.DisplayAlert(
                "Помилка", "Сталася помилка під час створення.", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
