using FoodDrinkApp.Models;
using FoodDrinkApp.Services;
using System.Text.Json;

namespace FoodDrinkApp.Pages;

[QueryProperty(nameof(FoodItemJson), "Food")]
public partial class FoodDetailPage : ContentPage
{
    private FoodItem _foodItem = new();

    public string FoodItemJson
    {
        set
        {
            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var decodedJson = Uri.UnescapeDataString(value);
                    _foodItem = JsonSerializer.Deserialize<FoodItem>(decodedJson) ?? new FoodItem();
                    BindingContext = _foodItem;
                    RenderPage();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                _foodItem = new FoodItem();
            }
        }
    }

    public FoodDetailPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _foodItem = FoodTransferService.SelectedFood ?? new FoodItem();
        BindingContext = _foodItem;
        RenderPage();
        AccessibilityService.ApplyFontScale(this);
    }

    protected override void OnDisappearing()
    {
        SpeechService.Stop();
        base.OnDisappearing();
    }

    private void RenderPage()
    {
        if (_foodItem == null) return;

        NameLabel.Text = _foodItem.Name ?? "Unknown";
        TypeLabel.Text = _foodItem.Type ?? "Unknown";
        StepsLabel.Text = _foodItem.Steps ?? "No steps provided.";

        if (!string.IsNullOrWhiteSpace(_foodItem.PhotoBase64))
        {
            try
            {
                var imageBytes = Convert.FromBase64String(_foodItem.PhotoBase64);
                FoodPhotoImage.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                FoodPhotoImage.IsVisible = true;
                FoodEmojiLabel.IsVisible = false;
            }
            catch
            {
                ShowEmoji();
            }
        }
        else
        {
            ShowEmoji();
        }
    }

    private void ShowEmoji()
    {
        FoodPhotoImage.IsVisible = false;
        FoodEmojiLabel.Text = _foodItem?.FoodEmoji ?? "🍲";
        FoodEmojiLabel.IsVisible = true;
    }

    // 朗读 - 使用 SpeechService
    private async void OnSpeakClicked(object sender, EventArgs e)
    {
        try
        {
            if (_foodItem == null)
            {
                await DisplayAlert("Error", "No food data available.", "OK");
                return;
            }

            var text = $"{_foodItem.Name}. {_foodItem.Type}. {_foodItem.Steps}";
            await SpeechService.SpeakAsync(text);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Text to speech unavailable", ex.Message, "OK");
        }
    }

    // 停止 - 使用 SpeechService
    private void OnStopSpeakingClicked(object sender, EventArgs e)
    {
        SpeechService.Stop();
        SemanticScreenReader.Announce("Reading stopped.");
    }
}