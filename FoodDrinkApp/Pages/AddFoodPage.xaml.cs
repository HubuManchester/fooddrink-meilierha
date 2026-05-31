using FoodDrinkApp.ViewModels;

namespace FoodDrinkApp.Pages;

public partial class AddFoodPage : ContentPage
{
    public AddFoodPage(AddFoodViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}