namespace ClientApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Головна "оболонка" – наш Shell з усіма сторінками
        MainPage = new AppShell();

        // (опціонально) одразу переходимо на LoginPage
        NavigateToLogin();
    }

    private async void NavigateToLogin()
    {
        // невелика затримка, щоб Shell встиг ініціалізуватися
        await Task.Delay(100);

        if (MainPage is Shell shell)
        {
            await shell.GoToAsync("//LoginPage");
        }
    }
}
