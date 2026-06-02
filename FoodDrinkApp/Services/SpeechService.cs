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
            var options = new SpeechOptions();
            await TextToSpeech.Default.SpeakAsync(text, options, currentSpeech.Token);
        }
        catch (OperationCanceledException)
        {
            // 用户取消了朗读
            System.Diagnostics.Debug.WriteLine("TTS cancelled by user");
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

        try
        {
            currentSpeech.Cancel();
            currentSpeech.Dispose();
        }
        catch
        {
            // 忽略
        }
        finally
        {
            currentSpeech = null;
        }
    }
}