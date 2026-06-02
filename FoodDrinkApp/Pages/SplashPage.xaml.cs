namespace FoodDrinkApp.Pages;

public partial class SplashPage : ContentPage
{
    public SplashPage()
    {
        InitializeComponent();
        LoadMainPage();
    }

    private async void LoadMainPage()
    {
        await Task.Delay(2000); // œ‘ æ2√Î∆Ù∂Ø“≥
        Application.Current.MainPage = new AppShell();
    }
}