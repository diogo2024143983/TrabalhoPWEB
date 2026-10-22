using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using MyHotels.Admin;
using MyHotels.Admin.Components;
using MyHotels.Domain.Identity;
using MyHotels.Infrastructure;
using MyHotels.Infrastructure.Data;
using MyHotels.Infrastructure.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();
builder.Services.AddMyHotelsData(builder.Configuration);
builder.Services.AddMyHotelsIdentityCore();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Staff", policy =>
        policy.RequireRole(AppRoles.Administrador, AppRoles.Funcionario));
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole(AppRoles.Administrador));
});

var app = builder.Build();

await Inicializacao.InicializarAsync(app.Services);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
