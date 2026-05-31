using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodDrinkApp.Models;
using FoodDrinkApp.Services;

namespace FoodDrinkApp.ViewModels;

public partial class AddFoodViewModel : BaseViewModel
{
    private readonly FoodService _foodService;

    public AddFoodViewModel(FoodService foodService)
    {
        _foodService = foodService;
    }

    [ObservableProperty]
    private string foodName = string.Empty;

    [ObservableProperty]
    private string foodType = string.Empty;

    [ObservableProperty]
    private string foodSteps = string.Empty;

    [RelayCommand]
    private async Task SubmitFood()
    {
        // 验证
        if (string.IsNullOrWhiteSpace(FoodName))
        {
            await Shell.Current.DisplayAlert("Validation Error", "Please enter a food name.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(FoodType))
        {
            await Shell.Current.DisplayAlert("Validation Error", "Please enter a food type.", "OK");
            return;
        }

        if (IsBusy) return;

        try
        {
            IsBusy = true;

            var newId = _foodService.Foods.Count + 1;

            var newFood = new FoodItem
            {
                Id = newId,
                Name = FoodName.Trim(),
                Type = FoodType.Trim(),
                Steps = string.IsNullOrWhiteSpace(FoodSteps) ? "No steps provided." : FoodSteps.Trim(),
                ImageFile = ""
            };

            // 使用 FoodService 的 AddFood 方法
            _foodService.AddFood(newFood);

            await Shell.Current.DisplayAlert("Success", $"\"{FoodName}\" has been added!", "OK");

            // 返回主页
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to add: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}