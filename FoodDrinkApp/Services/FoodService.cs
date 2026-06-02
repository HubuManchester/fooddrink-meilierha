using FoodDrinkApp.Models;
using System.Collections.ObjectModel;

namespace FoodDrinkApp.Services;

public class FoodService
{
    private ObservableCollection<FoodItem> foods;

    public ObservableCollection<FoodItem> Foods
    {
        get => foods;
        private set
        {
            foods = value;
            OnFoodsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler? OnFoodsChanged;

    public FoodService()
    {
        foods = new ObservableCollection<FoodItem>();
        LoadDefaultData();
    }

    private void LoadDefaultData()
    {
        foods.Add(new FoodItem
        {
            Id = 1,
            Name = "Tomato Egg",
            Type = "Home Cooking",
            Steps = "1. Cut tomatoes into pieces.\n2. Beat eggs in a bowl.\n3. Heat oil in a pan.\n4. Fry eggs and remove.\n5. Cook tomatoes until soft.\n6. Mix eggs back.\n7. Season and serve.",
            PhotoBase64 = ""
        });

        foods.Add(new FoodItem
        {
            Id = 2,
            Name = "Milk Tea",
            Type = "Drink",
            Steps = "1. Boil water.\n2. Add tea bags and steep.\n3. Add milk and sugar.\n4. Serve hot or over ice.",
            PhotoBase64 = ""
        });

        foods.Add(new FoodItem
        {
            Id = 3,
            Name = "Chocolate Cake",
            Type = "Dessert",
            Steps = "1. Preheat oven to 350°F.\n2. Mix dry ingredients.\n3. Add wet ingredients.\n4. Bake for 30 minutes.\n5. Let cool and serve.",
            PhotoBase64 = ""
        });
    }

    public void AddFood(FoodItem newFood)
    {
        newFood.Id = foods.Count + 1;
        foods.Add(newFood);
        OnFoodsChanged?.Invoke(this, EventArgs.Empty);
    }

    public void DeleteFood(int id)
    {
        var food = foods.FirstOrDefault(f => f.Id == id);
        if (food != null)
        {
            foods.Remove(food);
            OnFoodsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void UpdateFood(FoodItem updatedFood)
    {
        var index = foods.ToList().FindIndex(f => f.Id == updatedFood.Id);
        if (index >= 0)
        {
            foods[index] = updatedFood;
            OnFoodsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}