namespace FoodDrinkApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("FoodDetailPage", typeof(Pages.FoodDetailPage));
        Routing.RegisterRoute("AddFoodPage", typeof(Pages.AddFoodPage));
        Routing.RegisterRoute("EditFoodPage", typeof(Pages.EditFoodPage));
        Routing.RegisterRoute("SettingsPage", typeof(Pages.SettingsPage));
    }
}