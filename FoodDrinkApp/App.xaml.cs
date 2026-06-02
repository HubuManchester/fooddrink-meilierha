using FoodDrinkApp.Pages;
using Microsoft.Maui.Storage;

namespace FoodDrinkApp;

public partial class App : Application
{
    private const string ThemeKey = "app_theme";

    public App()
    {
        InitializeComponent();

        // 启动时读取并应用保存的主题
        var savedTheme = Preferences.Get(ThemeKey, 0);
        ApplyTheme(savedTheme);
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new SplashPage());
    }

    private void ApplyTheme(int themeIndex)
    {
        UserAppTheme = themeIndex switch
        {
            1 => AppTheme.Light,
            2 => AppTheme.Dark,
            _ => AppTheme.Unspecified
        };
    }
}