using FoodDrinkApp.Services;
using Microsoft.Maui.Storage;

namespace FoodDrinkApp.Pages;

public partial class SettingsPage : ContentPage
{
    private const string ThemeKey = "app_theme";

    public SettingsPage()
    {
        InitializeComponent();

        // 从 Preferences 读取保存的主题
        var savedTheme = Preferences.Get(ThemeKey, 0);
        ThemePicker.SelectedIndex = savedTheme;

        // 从 AccessibilityService 读取大字体设置
        LargeTextSwitch.IsToggled = AccessibilityService.LargeTextEnabled;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        AccessibilityService.ApplyFontScale(this);
        LargeTextSwitch.IsToggled = AccessibilityService.LargeTextEnabled;
        UpdatePreviewText();
    }

    private void OnThemeChanged(object sender, EventArgs e)
    {
        var themeIndex = ThemePicker.SelectedIndex;
        ApplyTheme(themeIndex);

        // 保存主题设置到 Preferences
        Preferences.Set(ThemeKey, themeIndex);

        StatusLabel.Text = $"Theme changed to {ThemePicker.SelectedItem}";
        SemanticScreenReader.Announce(StatusLabel.Text);
    }

    private void ApplyTheme(int themeIndex)
    {
        Application.Current!.UserAppTheme = themeIndex switch
        {
            1 => AppTheme.Light,
            2 => AppTheme.Dark,
            _ => AppTheme.Unspecified
        };
    }

    private void OnLargeTextToggled(object sender, ToggledEventArgs e)
    {
        AccessibilityService.LargeTextEnabled = e.Value;
        AccessibilityService.ApplyFontScale(this);
        UpdatePreviewText();

        StatusLabel.Text = e.Value ? "Large text mode: ON" : "Large text mode: OFF";
        SemanticScreenReader.Announce(StatusLabel.Text);
    }

    private void UpdatePreviewText()
    {
        if (AccessibilityService.LargeTextEnabled)
        {
            PreviewTitle.Text = "✨ Large text preview (ENLARGED)";
            PreviewTitle.FontSize = 22;
            PreviewBody.FontSize = 18;
            PreviewBody.Text = "This is how large text looks when the feature is enabled. The setting applies to all pages.";
        }
        else
        {
            PreviewTitle.Text = "Large text preview";
            PreviewTitle.FontSize = 18;
            PreviewBody.FontSize = 14;
            PreviewBody.Text = "When enabled, this text becomes visibly larger to demonstrate the accessibility feature.";
        }
    }
}