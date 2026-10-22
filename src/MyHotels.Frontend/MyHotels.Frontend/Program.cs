using MyHotels.Frontend.Components;
using MyHotels.Shared.Data.Interfaces;
using MyHotels.Shared.Data.Services;
using MyHotels.Shared.Pages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var apiBaseUrl = builder.Configuration["ApiBaseUrl"]
    ?? "https://localhost:7280/";

builder.Services.AddScoped<CartState>();
builder.Services.AddScoped<ITokenStore, BrowserTokenStore>();
builder.Services.AddTransient<JwtAuthorizationHandler>();
builder.Services.AddHttpClient<ICatalogApi, CatalogApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});
builder.Services.AddHttpClient<BookingApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<JwtAuthorizationHandler>();
builder.Services.AddHttpClient("MyHotels.Api", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<JwtAuthorizationHandler>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(CatalogHome).Assembly);

app.Run();
