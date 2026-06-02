using FoodDrinkApp.Models;
using FoodDrinkApp.Services;

namespace FoodDrinkApp.Pages;

public partial class EditFoodPage : ContentPage
{
    private FoodItem _foodItem = new();
    private byte[]? _photoBytes;
    private readonly FoodService _foodService;

    public EditFoodPage(FoodService foodService)
    {
        InitializeComponent();
        _foodService = foodService;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _foodItem = FoodTransferService.SelectedFood ?? new FoodItem();
        LoadData();
    }

    private void LoadData()
    {
        if (_foodItem == null) return;

        NameEntry.Text = _foodItem.Name;
        StepsEditor.Text = _foodItem.Steps;

        var categoryIndex = CategoryPicker.Items.ToList().IndexOf(_foodItem.Type);
        CategoryPicker.SelectedIndex = categoryIndex >= 0 ? categoryIndex : 0;

        if (!string.IsNullOrWhiteSpace(_foodItem.PhotoBase64))
        {
            try
            {
                var imageBytes = Convert.FromBase64String(_foodItem.PhotoBase64);
                FoodPhotoPreview.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
            }
            catch { }
        }
    }

    private async void OnTakePhotoClicked(object sender, EventArgs e)
    {
        try
        {
            // ¼ì²âÊÇ·ñÊÇÄ£ÄâÆ÷
            if (DeviceInfo.Current.DeviceType == DeviceType.Virtual)
            {
                await DisplayAlert("Camera Not Available", "You are using an emulator.\n\nCamera only works on real phones.", "OK");
                return;
            }

            if (!MediaPicker.Default.IsCaptureSupported)
            {
                await DisplayAlert("Not Supported", "This device does not support camera capture.", "OK");
                return;
            }

            var photo = await MediaPicker.Default.CapturePhotoAsync();
            if (photo is null) return;

            await using var stream = await photo.OpenReadAsync();
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            _photoBytes = memoryStream.ToArray();

            FoodPhotoPreview.Source = ImageSource.FromStream(() => new MemoryStream(_photoBytes));

            HapticFeedback.Default.Perform(HapticFeedbackType.Click);
        }
        catch (PermissionException)
        {
            await DisplayAlert("Permission Denied", "Camera permission was denied.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Camera Error", $"Failed to capture photo: {ex.Message}", "OK");
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        ValidationLabel.IsVisible = false;

        if (string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            ValidationLabel.Text = "Please enter a food name.";
            ValidationLabel.IsVisible = true;
            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(200));
            return;
        }

        if (CategoryPicker.SelectedIndex < 0)
        {
            ValidationLabel.Text = "Please select a category.";
            ValidationLabel.IsVisible = true;
            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(200));
            return;
        }

        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            _foodItem.Name = NameEntry.Text.Trim();
            _foodItem.Type = CategoryPicker.SelectedItem?.ToString() ?? "Uncategorized";
            _foodItem.Steps = string.IsNullOrWhiteSpace(StepsEditor.Text) ? "No steps." : StepsEditor.Text.Trim();

            if (_photoBytes != null)
            {
                _foodItem.PhotoBase64 = Convert.ToBase64String(_photoBytes);
            }

            _foodService.UpdateFood(_foodItem);

            HapticFeedback.Default.Perform(HapticFeedbackType.Click);
            await DisplayAlert("Success", $"\"{_foodItem.Name}\" has been updated!", "OK");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to update: {ex.Message}", "OK");
        }
        finally
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }
    }
}