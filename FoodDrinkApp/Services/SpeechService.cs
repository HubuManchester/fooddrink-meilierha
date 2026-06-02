namespace FoodDrinkApp.Services;

public static class SpeechService
{
    private static CancellationTokenSource? currentSpeech;

    public static async Task SpeakAsync(string text)
    {
        Stop();

        currentSpeech = new CancellationTokenSource();

        try
        {
            // 需要传 SpeechOptions 才能带 CancellationToken
            var options = new SpeechOptions();
            await TextToSpeech.Default.SpeakAsync(text, options, currentSpeech.Token);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"TTS Error: {ex.Message}");
            throw;
        }
    }

    public static void Stop()
    {
        if (currentSpeech is null) return;
        currentSpeech.Cancel();
        currentSpeech.Dispose();
        currentSpeech = null;
    }
}