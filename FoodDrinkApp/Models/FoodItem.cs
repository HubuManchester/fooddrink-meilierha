using System.Text.Json.Serialization;

namespace FoodDrinkApp.Models;

public class FoodItem
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("steps")]
    public string Steps { get; set; } = string.Empty;

    [JsonPropertyName("imageFile")]
    public string ImageFile { get; set; } = string.Empty;

    // 存储拍照的图片（Base64格式）
    [JsonPropertyName("photoBase64")]
    public string PhotoBase64 { get; set; } = string.Empty;

    // 根据食物类型返回对应 Emoji（作为默认图标）
    [JsonIgnore]
    public string FoodEmoji
    {
        get
        {
            if (Type.ToLower().Contains("drink")) return "🥤";
            if (Type.ToLower().Contains("dessert") || Type.ToLower().Contains("sweet")) return "🍰";
            if (Type.ToLower().Contains("breakfast")) return "🍳";
            if (Type.ToLower().Contains("lunch") || Type.ToLower().Contains("dinner")) return "🍽️";
            if (Type.ToLower().Contains("snack")) return "🍪";
            return "🍲";
        }
    }

    // 是否有拍照图片
    [JsonIgnore]
    public bool HasPhoto => !string.IsNullOrWhiteSpace(PhotoBase64);
}