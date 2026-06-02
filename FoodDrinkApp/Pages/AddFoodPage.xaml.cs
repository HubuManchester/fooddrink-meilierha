using FoodDrinkApp.Models;
using FoodDrinkApp.Services;

namespace FoodDrinkApp.Pages;

public partial class AddFoodPage : ContentPage
{
    private byte[]? _photoBytes;
    private readonly FoodService _foodService;

    public AddFoodPage(FoodService foodService)
    {
        InitializeComponent();
        _foodService = foodService;
    }

    // 拍照功能
    private async void OnTakePhotoClicked(object sender, EventArgs e)
    {
        try
        {
            // 检测是否是模拟器
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
            if (photo is null)
            {
                return;
            }

            await using var stream = await photo.OpenReadAsync();
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            _photoBytes = memoryStream.ToArray();

            // 使用 FoodPhotoPreview（与 XAML 中的 x:Name 一致）
            FoodPhotoPreview.Source = ImageSource.FromStream(() => new MemoryStream(_photoBytes));

            HapticFeedback.Default.Perform(HapticFeedbackType.Click);
        }
        catch (PermissionException)
        {
            await DisplayAlert("Permission Denied", "Camera permission was denied. Enable camera access in device settings.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Camera Error", $"Failed to capture photo: {ex.Message}", "OK");
        }
    }

    // 提交表单
    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        ValidationLabel.IsVisible = false;

        var validationError = ValidateForm();
        if (validationError != null)
        {
            ValidationLabel.Text = validationError;
            ValidationLabel.IsVisible = true;
            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(200));
            return;
        }

        if (LoadingIndicator.IsVisible) return;

        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            var newFood = new FoodItem
            {
                Name = NameEntry.Text!.Trim(),
                Type = CategoryPicker.SelectedItem?.ToString() ?? "Uncategorized",
                Steps = string.IsNullOrWhiteSpace(StepsEditor.Text) ? "No steps provided." : StepsEditor.Text.Trim(),
                PhotoBase64 = _photoBytes != null ? Convert.ToBase64String(_photoBytes) : ""
            };

            _foodService.AddFood(newFood);

            HapticFeedback.Default.Perform(HapticFeedbackType.Click);
            await DisplayAlert("Success", $"\"{newFood.Name}\" has been added!", "OK");

            // 清空表单
            NameEntry.Text = string.Empty;
            CategoryPicker.SelectedIndex = -1;
            StepsEditor.Text = string.Empty;
            _photoBytes = null;
            FoodPhotoPreview.Source = null;

            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to add: {ex.Message}", "OK");
        }
        finally
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }
    }

    private string? ValidateForm()
    {
        if (string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            return "Please enter a food name.";
        }

        if (CategoryPicker.SelectedIndex < 0)
        {
            return "Please select a category.";
        }

        return null;
    }
}