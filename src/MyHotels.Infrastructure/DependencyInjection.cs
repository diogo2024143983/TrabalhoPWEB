using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyHotels.Infrastructure.Data;
using MyHotels.Infrastructure.Identity;

namespace MyHotels.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddMyHotelsData(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(AppConfig.ConnectionStringName)
            ?? throw new InvalidOperationException($"Connection string '{AppConfig.ConnectionStringName}' was not configured.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        return services;
    }

    public static IdentityBuilder AddMyHotelsIdentityCore(this IServiceCollection services)
    {
        return services.AddIdentityCore<ApplicationUser>(ConfigureIdentity)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();
    }

    public static IdentityBuilder AddMyHotelsIdentity(this IServiceCollection services)
    {
        return services.AddIdentity<ApplicationUser, IdentityRole>(ConfigureIdentity)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
    }

    private static void ConfigureIdentity(IdentityOptions options)
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 8;
        options.SignIn.RequireConfirmedAccount = false;
    }
}
