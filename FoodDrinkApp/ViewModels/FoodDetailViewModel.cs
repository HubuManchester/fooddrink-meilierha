using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodDrinkApp.Models;
using Microsoft.Maui.ApplicationModel;


namespace FoodDrinkApp.ViewModels;

[QueryProperty(nameof(Food), "Food")]
public partial class FoodDetailViewModel : BaseViewModel
{
    [ObservableProperty]
    private FoodItem food = new();

    [RelayCommand]
    private async Task SpeakSteps()
    {
        try
        {
            if (Food == null)
            {
                await Shell.Current.DisplayAlert("Error", "No food data available.", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(Food.Steps))
            {
                await Shell.Current.DisplayAlert("Info", "No steps to read aloud.", "OK");
                return;
            }

            IsBusy = true;

            await TextToSpeech.SpeakAsync(Food.Steps, new SpeechOptions
            {
                Pitch = 1.0f,
                Volume = 1.0f
            });
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", $"Failed to read aloud: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}