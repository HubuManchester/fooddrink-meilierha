using System.Runtime.CompilerServices;
using Microsoft.Maui.Storage;

namespace FoodDrinkApp.Services;

public static class AccessibilityService
{
    private const double LargeTextScale = 1.22;
    private const string LargeTextKey = "large_text_enabled";
    private static readonly ConditionalWeakTable<BindableObject, FontSizeStore> OriginalFontSizes = new();

    private static bool _largeTextEnabled;

    public static bool LargeTextEnabled
    {
        get => _largeTextEnabled;
        set
        {
            _largeTextEnabled = value;
            // 保存到 Preferences
            Preferences.Set(LargeTextKey, value);
        }
    }

    static AccessibilityService()
    {
        // 从 Preferences 读取保存的大字体设置
        _largeTextEnabled = Preferences.Get(LargeTextKey, false);
    }

    public static void ApplyFontScale(Element root)
    {
        ApplyToElement(root);

        if (root is not IVisualTreeElement visualTreeElement)
        {
            return;
        }

        foreach (var child in visualTreeElement.GetVisualChildren().OfType<Element>())
        {
            ApplyFontScale(child);
        }
    }

    private static void ApplyToElement(Element element)
    {
        var scale = LargeTextEnabled ? LargeTextScale : 1.0;

        switch (element)
        {
            case Label label:
                label.FontSize = GetOriginalFontSize(label, label.FontSize) * scale;
                break;
            case Button button:
                button.FontSize = GetOriginalFontSize(button, button.FontSize) * scale;
                break;
            case Entry entry:
                entry.FontSize = GetOriginalFontSize(entry, entry.FontSize) * scale;
                break;
            case Editor editor:
                editor.FontSize = GetOriginalFontSize(editor, editor.FontSize) * scale;
                break;
            case Picker picker:
                picker.FontSize = GetOriginalFontSize(picker, picker.FontSize) * scale;
                break;
            case SearchBar searchBar:
                searchBar.FontSize = GetOriginalFontSize(searchBar, searchBar.FontSize) * scale;
                break;
        }
    }

    private static double GetOriginalFontSize(BindableObject control, double currentSize)
    {
        var store = OriginalFontSizes.GetOrCreateValue(control);
        if (!store.HasValue)
        {
            store.Value = currentSize > 0 ? currentSize : 14;
            store.HasValue = true;
        }

        return store.Value;
    }

    private sealed class FontSizeStore
    {
        public bool HasValue { get; set; }
        public double Value { get; set; }
    }
}