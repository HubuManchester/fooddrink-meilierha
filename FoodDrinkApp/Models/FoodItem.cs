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

    [JsonPropertyName("photoBase64")]
    public string PhotoBase64 { get; set; } = string.Empty;

    // 智能搜索标签（优化版）
    [JsonIgnore]
    public List<string> SmartTags
    {
        get
        {
            var tags = new List<string>();
            var nameLower = Name.ToLower();
            var typeLower = Type.ToLower();
            var stepsLower = Steps.ToLower();

            // ========== 辣味 (spicy) ==========
            var spicyKeywords = new[] { "spicy", "curry", "mapo", "sichuan", "chili", "szechuan" };
            if (spicyKeywords.Any(k => nameLower.Contains(k)))
            {
                tags.AddRange(new[] { "spicy", "hot" });
            }

            // ========== 热食 (hot/warm) ==========
            var isColdDrink = typeLower.Contains("drink") && (nameLower.Contains("ice") || nameLower.Contains("iced"));
            var isDessert = typeLower.Contains("dessert") || nameLower.Contains("cake") || nameLower.Contains("sorbet") || nameLower.Contains("yogurt");

            if (!isColdDrink && !isDessert)
            {
                if (nameLower.Contains("soup") || nameLower.Contains("grill") || nameLower.Contains("roast") ||
                    nameLower.Contains("stir") || nameLower.Contains("fried"))
                {
                    tags.Add("hot");
                }
                if (stepsLower.Contains("simmer") || stepsLower.Contains("grill") || stepsLower.Contains("roast") ||
                    stepsLower.Contains("stir-fry") || stepsLower.Contains("bake"))
                {
                    tags.Add("hot");
                }
            }

            // ========== 冷食 (cold/iced) ==========
            if (nameLower.Contains("ice") || nameLower.Contains("iced") || nameLower.Contains("cold") ||
                nameLower.Contains("frozen") || nameLower.Contains("sorbet") || nameLower.Contains("chill"))
            {
                tags.AddRange(new[] { "cold", "iced" });
            }
            if (nameLower.Contains("cold sesame noodles"))
            {
                tags.AddRange(new[] { "cold", "noodle" });
            }

            // ========== 甜食 (sweet) ==========
            var sweetKeywords = new[] { "sweet", "cake", "chocolate", "tiramisu", "donut", "honey",
                                        "sorbet", "yogurt", "parfait", "dessert", "mango", "berry" };
            if (sweetKeywords.Any(k => nameLower.Contains(k)) || typeLower.Contains("dessert"))
            {
                tags.Add("sweet");
            }

            // ========== 快餐/快速 (fast) ==========
            var fastKeywords = new[] { "fast", "quick", "wrap", "burger", "donut", "parfait" };
            if (fastKeywords.Any(k => nameLower.Contains(k)) || typeLower.Contains("fast") || typeLower.Contains("breakfast"))
            {
                tags.Add("fast");
            }

            // ========== 烧烤 (grill/roast) ==========
            if (nameLower.Contains("grill") || nameLower.Contains("roast"))
            {
                tags.AddRange(new[] { "grill", "roast" });
            }

            // ========== 汤类 (soup) ==========
            if (nameLower.Contains("soup"))
            {
                tags.Add("soup");
            }

            // ========== 健康 (healthy) ==========
            var healthyKeywords = new[] { "healthy", "salad", "quinoa", "fresh" };
            if (healthyKeywords.Any(k => nameLower.Contains(k)) || typeLower.Contains("healthy"))
            {
                tags.Add("healthy");
            }

            // ========== 主食 ==========
            if (nameLower.Contains("rice") || nameLower.Contains("noodle"))
            {
                tags.AddRange(new[] { "staple", "rice", "noodle" });
            }

            // ========== 早餐 ==========
            if (typeLower.Contains("breakfast") || nameLower.Contains("donut") || nameLower.Contains("parfait"))
            {
                tags.Add("breakfast");
            }

            return tags.Distinct().ToList();
        }
    }

    // 精确的食物图标
    [JsonIgnore]
    public string FoodEmoji
    {
        get
        {
            var nameLower = Name.ToLower();
            var typeLower = Type.ToLower();

            // 饮料类
            if (nameLower.Contains("milk tea")) return "🧋";
            if (nameLower.Contains("coffee")) return "☕";
            if (nameLower.Contains("smoothie")) return "🥤";
            if (typeLower.Contains("drink")) return "🥤";

            // 甜点类
            if (nameLower.Contains("cake")) return "🍰";
            if (nameLower.Contains("chocolate")) return "🍫";
            if (nameLower.Contains("tiramisu")) return "🍰";
            if (nameLower.Contains("donut")) return "🍩";
            if (nameLower.Contains("sorbet") || nameLower.Contains("yogurt")) return "🍧";
            if (nameLower.Contains("parfait")) return "🥣";
            if (typeLower.Contains("dessert")) return "🍰";

            // 主食类
            if (nameLower.Contains("rice")) return "🍚";
            if (nameLower.Contains("noodle") || nameLower.Contains("ramen")) return "🍜";
            if (nameLower.Contains("soup")) return "🥣";
            if (nameLower.Contains("curry")) return "🍛";
            if (nameLower.Contains("burger")) return "🍔";
            if (nameLower.Contains("wrap")) return "🌯";
            if (nameLower.Contains("salad")) return "🥗";
            if (nameLower.Contains("tofu")) return "🥢";

            // 肉类
            if (nameLower.Contains("chicken")) return "🍗";
            if (nameLower.Contains("beef")) return "🥩";
            if (nameLower.Contains("salmon") || nameLower.Contains("fish")) return "🐟";
            if (nameLower.Contains("egg")) return "🍳";

            // 早餐
            if (typeLower.Contains("breakfast")) return "🍳";

            // 默认
            return "🍲";
        }
    }

    [JsonIgnore]
    public bool HasPhoto => !string.IsNullOrWhiteSpace(PhotoBase64);
}