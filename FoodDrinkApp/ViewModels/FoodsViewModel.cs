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

    [RelayCommand]
    private async Task GoToDetail(FoodItem food)
    {
        if (food == null) return;

        var parameters = new Dictionary<string, object>
        {
            { "Food", food }
        };

        await Shell.Current.GoToAsync("FoodDetailPage", true, parameters);
    }

    [RelayCommand]
    private async Task GoToAdd()
    {
        await Shell.Current.GoToAsync("AddFoodPage");
    }
}