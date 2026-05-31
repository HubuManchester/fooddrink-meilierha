using FoodDrinkApp.Services;

namespace FoodDrinkApp.Pages;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
        ThemePicker.SelectedIndex = 0;
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
        Application.Current!.UserAppTheme = ThemePicker.SelectedIndex switch
        {
            1 => AppTheme.Light,
            2 => AppTheme.Dark,
            _ => AppTheme.Unspecified
        };

        StatusLabel.Text = $"Theme changed to {ThemePicker.SelectedItem}";
        SemanticScreenReader.Announce(StatusLabel.Text);
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