using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text.Json;
using ClientApp.Models;
using Microsoft.Maui.Storage;

namespace ClientApp.ViewModels;

public partial class ItemsViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<ItemModel> items = new();

    private const string BaseUrl = "https://localhost:7123";

    public ItemsViewModel()
    {
        _ = LoadItemsAsync(); // запускаємо асинхронно
    }

    private async Task LoadItemsAsync()
    {
        try
        {
            // 1. Отримати токен
            var token = Preferences.Default.Get("jwt_token", null as string);
            if (string.IsNullOrEmpty(token))
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Помилка", "Немає токена. Будь ласка, увійдіть знову.", "OK");

                await Shell.Current.GoToAsync("//LoginPage");
                return;
            }

            // 2. Підготувати HTTP клієнт
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            // 3. GET /api/items
            var response = await client.GetAsync($"{BaseUrl}/api/items");

            if (!response.IsSuccessStatusCode)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Помилка", "Не вдалося завантажити список занять.", "OK");
                return;
            }

            // 4. Розпарсити список
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<ItemModel>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (list == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Помилка", "Порожній список або помилка парсингу.", "OK");
                return;
            }

            // 5. Оновити колекцію
            Items.Clear();
            foreach (var item in list)
                Items.Add(item);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            await Application.Current.MainPage.DisplayAlert(
                "Помилка", "Сталася помилка під час завантаження.", "OK");
        }
    }
}
