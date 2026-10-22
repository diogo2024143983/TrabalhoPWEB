using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyHotels.Domain.Entities;
using MyHotels.Infrastructure.Identity;

namespace MyHotels.Infrastructure.Data;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Hotel> Hotels => Set<Hotel>();
    public DbSet<Accommodation> Accommodations => Set<Accommodation>();
    public DbSet<AccommodationCategory> AccommodationCategories => Set<AccommodationCategory>();
    public DbSet<AvailabilityMode> AvailabilityModes => Set<AvailabilityMode>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<BookingItem> BookingItems => Set<BookingItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Accommodation>()
            .Property(item => item.BasePrice)
            .HasPrecision(18, 2);
        builder.Entity<Accommodation>()
            .Property(item => item.FinalPrice)
            .HasPrecision(18, 2);
        builder.Entity<Booking>()
            .Property(item => item.Total)
            .HasPrecision(18, 2);
        builder.Entity<BookingItem>()
            .Property(item => item.UnitPrice)
            .HasPrecision(18, 2);

        builder.Entity<Accommodation>()
            .HasOne(item => item.Hotel)
            .WithMany(item => item.Accommodations)
            .HasForeignKey(item => item.HotelId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Accommodation>()
            .HasOne(item => item.Category)
            .WithMany(item => item.Accommodations)
            .HasForeignKey(item => item.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Accommodation>()
            .HasOne(item => item.AvailabilityMode)
            .WithMany(item => item.Accommodations)
            .HasForeignKey(item => item.AvailabilityModeId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<BookingItem>()
            .HasOne(item => item.Booking)
            .WithMany(item => item.Items)
            .HasForeignKey(item => item.BookingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Booking>()
            .Property(item => item.StockUpdated)
            .HasDefaultValue(false);
        builder.Entity<Booking>()
            .Property(item => item.DocumentsExpedited)
            .HasDefaultValue(false);
        builder.Entity<BookingItem>()
            .HasOne(item => item.Accommodation)
            .WithMany(item => item.BookingItems)
            .HasForeignKey(item => item.AccommodationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
