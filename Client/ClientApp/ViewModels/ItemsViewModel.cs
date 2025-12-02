using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text.Json;
using ClientApp.Models;

namespace ClientApp.ViewModels;

public partial class ItemsViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<ItemModel> items = new();

    private const string BaseUrl = "https://localhost:7123";

    public ItemsViewModel()
    {
        LoadItems();
    }

    private async void LoadItems()
    {
        var token = Preferences.Get("jwt_token", null);
        if (string.IsNullOrEmpty(token))
        {
            await Application.Current.MainPage.DisplayAlert(
                "Помилка", "Немає токена. Увійдіть знову.", "OK");
            return;
        }

        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync($"{BaseUrl}/api/items");

        if (!response.IsSuccessStatusCode)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Помилка", "Не вдалося завантажити список занять", "OK");
            return;
        }

        var json = await response.Content.ReadAsStringAsync();
        var list = JsonSerializer.Deserialize<List<ItemModel>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Items.Clear();
        foreach (var item in list)
            Items.Add(item);
    }
}
