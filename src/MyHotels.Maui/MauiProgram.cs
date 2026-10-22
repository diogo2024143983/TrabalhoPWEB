using Microsoft.Extensions.Logging;
using MyHotels.Shared.Data.Interfaces;
using MyHotels.Shared.Data.Services;

namespace MyHotels.Maui;

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
            });

        builder.Services.AddMauiBlazorWebView();
        builder.Services.AddScoped<CartState>();
        builder.Services.AddScoped<ITokenStore, BrowserTokenStore>();
        builder.Services.AddTransient<JwtAuthorizationHandler>();
        builder.Services.AddHttpClient<ICatalogApi, CatalogApiService>(client =>
        {
            // Em desenvolvimento, substituir pelo Dev Tunnel da API.
            client.BaseAddress = new Uri(
                Environment.GetEnvironmentVariable("MYHOTELS_API_URL")
                ?? "https://localhost:7280/");
        }).AddHttpMessageHandler<JwtAuthorizationHandler>();
        builder.Services.AddHttpClient<BookingApiService>(client =>
        {
            client.BaseAddress = new Uri(
                Environment.GetEnvironmentVariable("MYHOTELS_API_URL")
                ?? "https://localhost:7280/");
        }).AddHttpMessageHandler<JwtAuthorizationHandler>();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
