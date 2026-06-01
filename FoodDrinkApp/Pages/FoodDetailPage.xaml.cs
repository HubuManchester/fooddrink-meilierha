using FoodDrinkApp.Models;
using FoodDrinkApp.Services;
using System.Text.Json;

namespace FoodDrinkApp.Pages;

[QueryProperty(nameof(FoodItemJson), "Food")]
public partial class FoodDetailPage : ContentPage
{
    private FoodItem _foodItem = new();
    private CancellationTokenSource? _cancellationTokenSource;

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
                System.Diagnostics.Debug.WriteLine($"Error deserializing: {ex.Message}");
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
        AccessibilityService.ApplyFontScale(this);
    }

    protected override void OnDisappearing()
    {
        StopSpeaking();
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

    private async void OnSpeakClicked(object sender, EventArgs e)
    {
        try
        {
            if (_foodItem == null)
            {
                await DisplayAlert("Error", "No food data available.", "OK");
                return;
            }

            StopSpeaking();

            _cancellationTokenSource = new CancellationTokenSource();

            var text = $"Food name: {_foodItem.Name}. Type: {_foodItem.Type}. Steps: {_foodItem.Steps}";

            await TextToSpeech.SpeakAsync(text, _cancellationTokenSource.Token);
        }
        catch (OperationCanceledException)
        {
            // 用户取消了
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to read aloud: {ex.Message}", "OK");
        }
    }

    private void OnStopSpeakingClicked(object sender, EventArgs e)
    {
        StopSpeaking();
    }

    private void StopSpeaking()
    {
        try
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
        catch { }
    }
}