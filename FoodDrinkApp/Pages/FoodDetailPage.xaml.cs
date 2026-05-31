using FoodDrinkApp.ViewModels;

namespace FoodDrinkApp.Pages;

public partial class FoodDetailPage : ContentPage
{
    public FoodDetailPage(FoodDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}