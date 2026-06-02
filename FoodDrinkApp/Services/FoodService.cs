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
            Name = "Tomato Egg Stir Fry",
            Type = "Home Cooking",
            Steps = "1. Cut tomatoes into pieces.\n2. Beat eggs in a bowl.\n3. Heat oil in a pan.\n4. Fry eggs and remove.\n5. Cook tomatoes until soft.\n6. Mix eggs back.\n7. Season and serve.",
            PhotoBase64 = ""
        });

        foods.Add(new FoodItem
        {
            Id = 2,
            Name = "Iced Milk Tea",
            Type = "Drink",
            Steps = "1. Boil water.\n2. Add tea bags and steep.\n3. Add milk and sugar.\n4. Pour over ice and serve cold.",
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

        foods.Add(new FoodItem
        {
            Id = 4,
            Name = "Spicy Mapo Tofu",
            Type = "Home Cooking",
            Steps = "1. Cut tofu into cubes.\n2. Prepare spicy bean paste.\n3. Stir-fry with minced pork.\n4. Add tofu and simmer.\n5. Thicken with cornstarch.\n6. Serve hot with rice.",
            PhotoBase64 = ""
        });

        foods.Add(new FoodItem
        {
            Id = 5,
            Name = "Spicy Beef Noodle Soup",
            Type = "Dinner",
            Steps = "1. Brown beef chunks.\n2. Add chili bean paste.\n3. Simmer with broth and spices.\n4. Cook noodles separately.\n5. Combine and top with green onions.\n6. Serve hot.",
            PhotoBase64 = ""
        });

        foods.Add(new FoodItem
        {
            Id = 6,
            Name = "Hot and Sour Soup",
            Type = "Dinner",
            Steps = "1. Prepare chicken broth.\n2. Add tofu, mushrooms, bamboo shoots.\n3. Season with vinegar and pepper.\n4. Thicken with egg.\n5. Serve immediately while hot.",
            PhotoBase64 = ""
        });

        foods.Add(new FoodItem
        {
            Id = 7,
            Name = "Grilled Salmon Steak",
            Type = "Dinner",
            Steps = "1. Season salmon with salt and pepper.\n2. Heat grill pan.\n3. Grill each side for 4-5 minutes.\n4. Serve with roasted vegetables.",
            PhotoBase64 = ""
        });

        foods.Add(new FoodItem
        {
            Id = 8,
            Name = "Roasted Chicken Leg",
            Type = "Lunch",
            Steps = "1. Marinate chicken with herbs.\n2. Preheat oven to 400°F.\n3. Roast for 35 minutes.\n4. Rest for 5 minutes before serving.",
            PhotoBase64 = ""
        });

        foods.Add(new FoodItem
        {
            Id = 9,
            Name = "Cold Sesame Noodles",
            Type = "Lunch",
            Steps = "1. Cook noodles and rinse with cold water.\n2. Mix sesame paste with soy sauce and vinegar.\n3. Toss noodles with sauce.\n4. Chill in refrigerator.\n5. Serve cold with cucumber.",
            PhotoBase64 = ""
        });

        foods.Add(new FoodItem
        {
            Id = 10,
            Name = "Frozen Yogurt",
            Type = "Dessert",
            Steps = "1. Mix yogurt with honey.\n2. Pour into container.\n3. Freeze for 4 hours.\n4. Scoop and serve cold.",
            PhotoBase64 = ""
        });

        foods.Add(new FoodItem
        {
            Id = 11,
            Name = "Mango Sorbet",
            Type = "Dessert",
            Steps = "1. Blend ripe mangoes with sugar.\n2. Strain the mixture.\n3. Churn in ice cream maker.\n4. Freeze until firm.\n5. Serve as a cold dessert.",
            PhotoBase64 = ""
        });

        foods.Add(new FoodItem
        {
            Id = 12,
            Name = "Tiramisu",
            Type = "Dessert",
            Steps = "1. Brew strong coffee.\n2. Whisk mascarpone with egg yolks and sugar.\n3. Dip ladyfingers in coffee.\n4. Layer cream and ladyfingers.\n5. Refrigerate for 4 hours.\n6. Dust with cocoa powder.",
            PhotoBase64 = ""
        });

        foods.Add(new FoodItem
        {
            Id = 13,
            Name = "Honey Glazed Donut",
            Type = "Breakfast",
            Steps = "1. Prepare dough and let rise.\n2. Cut into rings.\n3. Fry until golden.\n4. Dip in honey glaze.\n5. Serve warm.",
            PhotoBase64 = ""
        });

        foods.Add(new FoodItem
        {
            Id = 14,
            Name = "Cheeseburger",
            Type = "Fast Food",
            Steps = "1. Grill beef patty.\n2. Toast the bun.\n3. Add cheese, lettuce, tomato.\n4. Assemble and serve quickly.",
            PhotoBase64 = ""
        });

        foods.Add(new FoodItem
        {
            Id = 15,
            Name = "Chicken Wrap",
            Type = "Fast Food",
            Steps = "1. Grill chicken breast.\n2. Slice into strips.\n3. Wrap in tortilla with lettuce and sauce.\n4. Ready to eat in minutes.",
            PhotoBase64 = ""
        });

        foods.Add(new FoodItem
        {
            Id = 16,
            Name = "Quinoa Salad Bowl",
            Type = "Healthy",
            Steps = "1. Cook quinoa.\n2. Chop fresh vegetables.\n3. Mix with lemon dressing.\n4. Top with avocado and seeds.\n5. Serve fresh and healthy.",
            PhotoBase64 = ""
        });

        foods.Add(new FoodItem
        {
            Id = 17,
            Name = "Greek Yogurt Parfait",
            Type = "Breakfast",
            Steps = "1. Layer Greek yogurt.\n2. Add granola.\n3. Add fresh berries.\n4. Drizzle with honey.\n5. A healthy breakfast option.",
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