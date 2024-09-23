using Caesar.App.Services;
using Caesar.App.ViewModels;
using Caesar.App.Views;
using Microsoft.Extensions.Logging;

namespace Caesar.App;

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


        builder.Services.AddSingleton<ITokenService, TokenService>();
        builder.Services.AddSingleton<IApiService, ApiService>();

        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<MenuItemViewModel>();
        builder.Services.AddTransient<MenuItemDetailViewModel>();

        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<MenuItemDetailPage>();

        builder.Services.AddHttpClient();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}

