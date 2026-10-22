using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyHotels.Domain.Entities;
using MyHotels.Domain.Identity;
using MyHotels.Infrastructure.Identity;

namespace MyHotels.Infrastructure.Data;

public static class Inicializacao
{
    public static async Task InicializarAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var provider = scope.ServiceProvider;
        var db = provider.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync();

        var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
        foreach (var role in AppRoles.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
        await EnsureUserAsync(
            userManager,
            AppConfig.AdminEmail,
            AppConfig.AdminPassword,
            "Administrador MyHotels",
            AppRoles.Administrador,
            CustomerStatus.Active,
            isActive: true);
        await EnsureUserAsync(
            userManager,
            AppConfig.StaffEmail,
            AppConfig.StaffPassword,
            "Funcionário MyHotels",
            AppRoles.Funcionario,
            CustomerStatus.Active,
            isActive: true);
        await EnsureUserAsync(
            userManager,
            AppConfig.ClienteEmail,
            AppConfig.ClientePassword,
            "Cliente Demonstração",
            AppRoles.Cliente,
            CustomerStatus.Active,
            isActive: true,
            firstName: "Ana",
            lastName: "Cliente",
            nif: "123456789");
        await EnsureUserAsync(
            userManager,
            AppConfig.FornecedorEmail,
            AppConfig.FornecedorPassword,
            "Fornecedor Demonstração",
            AppRoles.Fornecedor,
            CustomerStatus.Active,
            isActive: true);

        if (await db.Hotels.AnyAsync())
        {
            return;
        }

        var categories = new[]
        {
            new AccommodationCategory { Name = "Quarto Standard", Description = "Quartos clássicos", IsActive = true },
            new AccommodationCategory { Name = "Suite", Description = "Suites com sala", IsActive = true },
            new AccommodationCategory { Name = "Apartamento", Description = "Apartamentos com cozinha", IsActive = true }
        };
        var modes = new[]
        {
            new AvailabilityMode { Name = "Reserva online", Description = "Disponível para reserva no frontend", IsActive = true },
            new AvailabilityMode { Name = "Apenas listagem", Description = "Visível sem reserva imediata", IsActive = true },
            new AvailabilityMode { Name = "Balcão", Description = "Só no hotel", IsActive = true }
        };

        db.AccommodationCategories.AddRange(categories);
        db.AvailabilityModes.AddRange(modes);
        await db.SaveChangesAsync();

        var hotels = new[]
        {
            new Hotel
            {
                Name = "MyHotels Coimbra",
                City = "Coimbra",
                Description = "Hotel urbano junto à baixa.",
                Stars = 4,
                ImagePath = "/img/hotels/coimbra.jpg",
                IsActive = true
            },
            new Hotel
            {
                Name = "MyHotels Lisboa Centro",
                City = "Lisboa",
                Description = "Aparthotel no centro da capital.",
                Stars = 5,
                ImagePath = "/img/hotels/lisboa.jpg",
                IsActive = true
            },
            new Hotel
            {
                Name = "MyHotels Porto Ribeira",
                City = "Porto",
                Description = "Vista sobre o Douro.",
                Stars = 4,
                ImagePath = "/img/hotels/porto.jpg",
                IsActive = true
            }
        };
        db.Hotels.AddRange(hotels);
        await db.SaveChangesAsync();

        var markup = 1 + (AppConfig.DefaultMarkupPercent / 100m);
        db.Accommodations.AddRange(
            new Accommodation
            {
                HotelId = hotels[0].Id,
                CategoryId = categories[0].Id,
                AvailabilityModeId = modes[0].Id,
                Name = "Quarto Duplo Rio",
                Description = "Vista parcial para o Mondego.",
                Capacity = 2,
                Stock = 8,
                BasePrice = 70m,
                FinalPrice = Math.Round(70m * markup, 2),
                ImagePath = "/img/rooms/duplo.jpg",
                IsListed = true,
                IsAvailableForBooking = true,
                IsActive = true
            },
            new Accommodation
            {
                HotelId = hotels[0].Id,
                CategoryId = categories[1].Id,
                AvailabilityModeId = modes[0].Id,
                Name = "Suite Executiva",
                Description = "Suite com sala de estar.",
                Capacity = 3,
                Stock = 3,
                BasePrice = 120m,
                FinalPrice = Math.Round(120m * markup, 2),
                ImagePath = "/img/rooms/suite.jpg",
                IsListed = true,
                IsAvailableForBooking = true,
                IsActive = true
            },
            new Accommodation
            {
                HotelId = hotels[1].Id,
                CategoryId = categories[2].Id,
                AvailabilityModeId = modes[0].Id,
                Name = "Apartamento T1 Chiado",
                Description = "Apartamento com cozinha equipada.",
                Capacity = 4,
                Stock = 5,
                BasePrice = 95m,
                FinalPrice = Math.Round(95m * markup, 2),
                ImagePath = "/img/rooms/apartamento.jpg",
                IsListed = true,
                IsAvailableForBooking = true,
                IsActive = true
            },
            new Accommodation
            {
                HotelId = hotels[2].Id,
                CategoryId = categories[0].Id,
                AvailabilityModeId = modes[1].Id,
                Name = "Quarto Twin Ribeira",
                Description = "Listagem promocional sem reserva online.",
                Capacity = 2,
                Stock = 10,
                BasePrice = 80m,
                FinalPrice = Math.Round(80m * markup, 2),
                ImagePath = "/img/rooms/twin.jpg",
                IsListed = true,
                IsAvailableForBooking = false,
                IsActive = true
            });

        await db.SaveChangesAsync();
    }

    private static async Task EnsureUserAsync(
        UserManager<ApplicationUser> userManager,
        string email,
        string password,
        string displayName,
        string role,
        CustomerStatus customerStatus,
        bool isActive,
        string? firstName = null,
        string? lastName = null,
        string? nif = null)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                DisplayName = displayName,
                FirstName = firstName,
                LastName = lastName,
                Nif = nif,
                IsActive = isActive,
                CustomerStatus = customerStatus
            };

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Não foi possível criar o utilizador {email}: {string.Join("; ", result.Errors.Select(e => e.Description))}");
            }
        }

        if (!await userManager.IsInRoleAsync(user, role))
        {
            await userManager.AddToRoleAsync(user, role);
        }
    }
}
