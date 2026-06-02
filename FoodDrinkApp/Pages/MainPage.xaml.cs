using FoodDrinkApp.Services;
using FoodDrinkApp.ViewModels;
using Microsoft.Maui.Devices.Sensors;

namespace FoodDrinkApp.Pages;

public partial class MainPage : ContentPage
{
    private FoodsViewModel _viewModel;
    private CancellationTokenSource _locationCancellation;
    private DateTime _lastShakeTime;
    private const double ShakeThreshold = 2.5;

    public MainPage(FoodsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        FoodCollection.ItemsSource = _viewModel.Foods;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        AccessibilityService.ApplyFontScale(this);

        // 每次页面出现时强制刷新数据
        _viewModel.ForceRefresh();

        // 每次页面出现时重新启动加速度计
        StartAccelerometer();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _locationCancellation?.Cancel();

        // 页面离开时停止加速度计
        StopAccelerometer();
    }

    private void StartAccelerometer()
    {
        try
        {
            if (Accelerometer.Default.IsSupported)
            {
                if (Accelerometer.Default.IsMonitoring)
                {
                    Accelerometer.Default.Stop();
                    Accelerometer.Default.ReadingChanged -= OnAccelerometerReadingChanged;
                }

                Accelerometer.Default.ReadingChanged += OnAccelerometerReadingChanged;
                Accelerometer.Default.Start(SensorSpeed.UI);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"StartAccelerometer error: {ex.Message}");
        }
    }

    private void StopAccelerometer()
    {
        try
        {
            if (Accelerometer.Default.IsSupported && Accelerometer.Default.IsMonitoring)
            {
                Accelerometer.Default.ReadingChanged -= OnAccelerometerReadingChanged;
                Accelerometer.Default.Stop();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"StopAccelerometer error: {ex.Message}");
        }
    }

    private void OnAccelerometerReadingChanged(object sender, AccelerometerChangedEventArgs e)
    {
        var data = e.Reading;
        var totalForce = Math.Abs(data.Acceleration.X) +
                         Math.Abs(data.Acceleration.Y) +
                         Math.Abs(data.Acceleration.Z);

        if (totalForce > ShakeThreshold)
        {
            var now = DateTime.Now;
            if ((now - _lastShakeTime).TotalMilliseconds > 1000)
            {
                _lastShakeTime = now;
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await OnShakeDetected();
                });
            }
        }
    }

    private async Task OnShakeDetected()
    {
        try
        {
            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(200));
            HapticFeedback.Default.Perform(HapticFeedbackType.Click);

            if (_viewModel.Foods.Any())
            {
                var random = new Random();
                var randomFood = _viewModel.Foods[random.Next(_viewModel.Foods.Count)];

                var shouldOpen = await DisplayAlert("🎲 Shake Detected!",
                    $"Random recommendation: {randomFood.Name}\n\nWould you like to view details?",
                    "Yes", "No");

                if (shouldOpen)
                {
                    FoodTransferService.SelectedFood = randomFood;
                    await Shell.Current.GoToAsync("FoodDetailPage");
                }
            }
            else
            {
                await DisplayAlert("🎲 Shake Detected!",
                    "No foods available. Please add some foods first.", "OK");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"OnShakeDetected error: {ex.Message}");
        }
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        _viewModel.SearchText = e.NewTextValue;
        _viewModel.FilterFoods();
    }

    private void OnSearchButtonPressed(object sender, EventArgs e)
    {
        _viewModel.SearchText = SearchBar.Text;
        _viewModel.FilterFoods();
    }

    private async void OnAddClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("AddFoodPage");
    }

    private async void OnDetailsClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is FoodDrinkApp.Models.FoodItem food)
        {
            FoodTransferService.SelectedFood = food;
            await Shell.Current.GoToAsync("FoodDetailPage");
        }
    }

    private async void OnRefreshing(object sender, EventArgs e)
    {
        _viewModel.LoadFoods();
        await Task.Delay(500);
        FoodRefreshView.IsRefreshing = false;
        SemanticScreenReader.Announce("Food list refreshed.");
    }

    private async void OnGetLocationClicked(object sender, EventArgs e)
    {
        try
        {
            LocationLabel.Text = "Getting location...";

            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            _locationCancellation = new CancellationTokenSource();

            var location = await Geolocation.Default.GetLocationAsync(request, _locationCancellation.Token);

            if (location is null)
            {
                LocationLabel.Text = "Location not available";
                return;
            }

            LocationLabel.Text = $"Lat: {location.Latitude:F4}, Lon: {location.Longitude:F4}";

            try
            {
                var placemarks = await Geocoding.Default.GetPlacemarksAsync(location);
                var placemark = placemarks?.FirstOrDefault();
                if (placemark != null)
                {
                    var address = string.Join(", ", new[]
                    {
                        placemark.Locality,
                        placemark.AdminArea,
                        placemark.CountryName
                    }.Where(x => !string.IsNullOrWhiteSpace(x)));

                    if (!string.IsNullOrWhiteSpace(address))
                    {
                        LocationLabel.Text = address;
                    }
                }
            }
            catch { }

            HapticFeedback.Default.Perform(HapticFeedbackType.Click);
            SemanticScreenReader.Announce($"Your location is {LocationLabel.Text}");
        }
        catch (PermissionException)
        {
            LocationLabel.Text = "Location permission denied";
            await DisplayAlert("Permission Required", "Please enable location access in settings.", "OK");
        }
        catch (Exception ex)
        {
            LocationLabel.Text = "Location unavailable";
            await DisplayAlert("Error", $"Could not get location: {ex.Message}", "OK");
        }
    }
}