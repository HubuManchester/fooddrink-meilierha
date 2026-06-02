using FoodDrinkApp.Pages;
using FoodDrinkApp.Services;
using FoodDrinkApp.ViewModels;
using Microsoft.Extensions.Logging;

namespace FoodDrinkApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // 注册服务
        builder.Services.AddSingleton<FoodService>();

        // 注册 ViewModel
        builder.Services.AddSingleton<FoodsViewModel>();
        builder.Services.AddTransient<AddFoodViewModel>();
        builder.Services.AddTransient<FoodDetailViewModel>();

        // 注册页面
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<AddFoodPage>();
        builder.Services.AddTransient<FoodDetailPage>();
        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<EditFoodPage>();  // 确保这行存在

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}