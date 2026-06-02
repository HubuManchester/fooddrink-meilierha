using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodDrinkApp.Models;
using FoodDrinkApp.Services;
using System.Collections.ObjectModel;

namespace FoodDrinkApp.ViewModels;

public partial class FoodsViewModel : BaseViewModel
{
    private readonly FoodService _foodService;
    private List<FoodItem> _allFoods = new();

    [ObservableProperty]
    private ObservableCollection<FoodItem> foods = new();

    [ObservableProperty]
    private string searchText = string.Empty;

    public FoodsViewModel(FoodService foodService)
    {
        _foodService = foodService;
        Title = "Food & Drink App";

        _foodService.OnFoodsChanged += (s, e) =>
        {
            _allFoods = _foodService.Foods.ToList();
            FilterFoods();
        };

        LoadFoods();
    }

    public void LoadFoods()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            _allFoods = _foodService.Foods.ToList();
            FilterFoods();
        }
        finally
        {
            IsBusy = false;
        }
    }

    public void FilterFoods()
    {
        var filtered = _allFoods.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var searchLower = SearchText.ToLower();
            filtered = filtered.Where(f =>
                (f.Name?.ToLower().Contains(searchLower) ?? false) ||
                (f.Type?.ToLower().Contains(searchLower) ?? false) ||
                (f.Steps?.ToLower().Contains(searchLower) ?? false));
        }

        Foods.Clear();
        foreach (var food in filtered.OrderBy(f => f.Name))
        {
            Foods.Add(food);
        }
    }

    // 点击查看详情 - 使用静态变量传递，不通过URL
    [RelayCommand]
    private async Task GoToDetail(FoodItem food)
    {
        if (food == null) return;

        FoodTransferService.SelectedFood = food;
        await Shell.Current.GoToAsync("FoodDetailPage");
    }

    // 编辑食物
    [RelayCommand]
    private async Task EditFood(FoodItem food)
    {
        if (food == null) return;

        FoodTransferService.SelectedFood = food;
        await Shell.Current.GoToAsync("EditFoodPage");
    }

    // 删除食物
    [RelayCommand]
    private async Task DeleteFood(FoodItem food)
    {
        if (food == null) return;

        var confirm = await Shell.Current.DisplayAlert("Confirm Delete",
            $"Are you sure you want to delete \"{food.Name}\"?",
            "Yes", "No");

        if (confirm)
        {
            _foodService.DeleteFood(food.Id);
            LoadFoods();
            await Shell.Current.DisplayAlert("Deleted", $"\"{food.Name}\" has been deleted.", "OK");
        }
    }

    [RelayCommand]
    private async Task GoToAdd()
    {
        await Shell.Current.GoToAsync("AddFoodPage");
    }
}