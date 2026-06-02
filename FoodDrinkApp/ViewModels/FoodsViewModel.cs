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

    // 智能搜索关键词映射（仅英文）
    private readonly Dictionary<string, string[]> _smartSearchMap = new()
    {
        { "spicy", new[] { "spicy", "hot", "chili", "pepper", "sichuan", "curry" } },
        { "hot", new[] { "hot", "spicy", "soup", "steam", "grill", "roast", "warm" } },
        { "cold", new[] { "cold", "ice", "iced", "cool", "chill", "frozen" } },
        { "sweet", new[] { "sweet", "dessert", "cake", "chocolate", "honey", "sugar" } },
        { "fast", new[] { "fast", "quick", "instant", "ready", "simple", "easy" } },
        { "healthy", new[] { "healthy", "fresh", "salad", "vegan", "light", "organic" } },
        { "grill", new[] { "grill", "roast", "barbecue", "bbq", "charcoal" } },
        { "soup", new[] { "soup", "broth", "stew", "warm", "hot" } },
        { "rice", new[] { "rice", "grain", "staple", "bowl" } },
        { "noodle", new[] { "noodle", "pasta", "ramen", "udon" } },
        { "breakfast", new[] { "breakfast", "morning", "egg", "toast", "cereal" } },
        { "lunch", new[] { "lunch", "dinner", "meal", "main", "entree" } }
    };

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

    // 添加强制刷新方法
    public void ForceRefresh()
    {
        LoadFoods();
    }

    public void FilterFoods()
    {
        var filtered = _allFoods.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var searchLower = SearchText.ToLower().Trim();

            if (_smartSearchMap.TryGetValue(searchLower, out var mappedTags))
            {
                filtered = filtered.Where(f =>
                    f.Name.ToLower().Contains(searchLower) ||
                    f.Type.ToLower().Contains(searchLower) ||
                    f.Steps.ToLower().Contains(searchLower) ||
                    mappedTags.Any(tag => f.Name.ToLower().Contains(tag) || f.Type.ToLower().Contains(tag)));
            }
            else
            {
                filtered = filtered.Where(f =>
                    f.Name.ToLower().Contains(searchLower) ||
                    f.Type.ToLower().Contains(searchLower) ||
                    f.Steps.ToLower().Contains(searchLower));
            }
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

        FoodTransferService.SelectedFood = food;
        await Shell.Current.GoToAsync("FoodDetailPage");
    }

    [RelayCommand]
    private async Task EditFood(FoodItem food)
    {
        if (food == null) return;

        FoodTransferService.SelectedFood = food;
        await Shell.Current.GoToAsync("EditFoodPage");
    }

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
        }
    }

    [RelayCommand]
    private async Task GoToAdd()
    {
        await Shell.Current.GoToAsync("AddFoodPage");
    }
}