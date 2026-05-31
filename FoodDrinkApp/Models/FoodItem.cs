namespace FoodDrinkApp.Models;

public class FoodItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Steps { get; set; } = string.Empty;
  
    public string ImageFile { get; set; } = string.Empty;
}