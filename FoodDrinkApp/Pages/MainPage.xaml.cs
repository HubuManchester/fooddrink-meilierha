using FoodDrinkApp.Services;
using FoodDrinkApp.ViewModels;
using System.Security.Cryptography;

namespace FoodDrinkApp.Pages;

public partial class MainPage : ContentPage
{
    private FoodsViewModel _viewModel;
    private CancellationTokenSource _locationCancellation;

    public MainPage(FoodsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

        // 绑定食物列表
        FoodCollection.ItemsSource = _viewModel.Foods;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        AccessibilityService.ApplyFontScale(this);
        _viewModel.LoadFoods();
    }

    protected override void OnDisappearing()
    {
        _locationCancellation?.Cancel();
        base.OnDisappearing();
    }

    // 搜索功能
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

    // 添加按钮
    private async void OnAddClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("AddFoodPage");
    }

    // 详情按钮
    private async void OnDetailsClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is FoodDrinkApp.Models.FoodItem food)
        {
            var parameters = new Dictionary<string, object>
            {
                { "Food", food }
            };
            await Shell.Current.GoToAsync("FoodDetailPage", true, parameters);
        }
    }

    // 下拉刷新
    private async void OnRefreshing(object sender, EventArgs e)
    {
        _viewModel.LoadFoods();
        await Task.Delay(500);
        FoodRefreshView.IsRefreshing = false;
        SemanticScreenReader.Announce("Food list refreshed.");
    }

    // 定位功能
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

            // 显示坐标
            LocationLabel.Text = $"Lat: {location.Latitude:F4}, Lon: {location.Longitude:F4}";

            // 尝试获取地址
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
            catch { /* 如果地理编码失败，只显示坐标 */ }

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