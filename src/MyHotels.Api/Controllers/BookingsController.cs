using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyHotels.Domain.Entities;
using MyHotels.Domain.Identity;
using MyHotels.Infrastructure.Data;

namespace MyHotels.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = AppRoles.Cliente)]
public sealed class BookingsController(ApplicationDbContext db) : ControllerBase
{
    public sealed record BookingItemRequest(int AccommodationId, int Quantity);

    public sealed record CreateBookingRequest(
        DateOnly CheckIn,
        DateOnly CheckOut,
        IReadOnlyList<BookingItemRequest> Items);

    [HttpGet("mine")]
    public async Task<IActionResult> GetMine(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var bookings = await db.Bookings
            .AsNoTracking()
            .Include(x => x.Items)
            .Where(x => x.CustomerId == userId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => new
            {
                x.Id,
                x.CheckIn,
                x.CheckOut,
                x.Status,
                x.Total,
                x.CreatedAtUtc,
                x.DocumentsExpedited,
                Items = x.Items.Select(i => new { i.AccommodationId, i.Quantity, i.UnitPrice })
            })
            .ToListAsync(cancellationToken);

        return Ok(bookings);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookingRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        if (request.Items is null
            || request.Items.Count == 0
            || request.CheckIn < DateOnly.FromDateTime(DateTime.UtcNow)
            || request.CheckOut <= request.CheckIn)
        {
            return BadRequest(new { message = "Pedido de reserva inválido." });
        }

        var requestedItems = request.Items
            .GroupBy(item => item.AccommodationId)
            .Select(group => new BookingItemRequest(group.Key, group.Sum(item => item.Quantity)))
            .ToList();
        if (requestedItems.Any(item => item.Quantity <= 0))
        {
            return BadRequest(new { message = "As quantidades devem ser positivas." });
        }

        var accommodationIds = requestedItems.Select(x => x.AccommodationId).ToList();
        var accommodations = await db.Accommodations
            .Include(x => x.Hotel)
            .Where(x => accommodationIds.Contains(x.Id)
                        && x.IsActive
                        && x.IsAvailableForBooking
                        && x.Hotel != null
                        && x.Hotel.IsActive)
            .ToListAsync(cancellationToken);

        if (accommodations.Count != accommodationIds.Count)
        {
            return BadRequest(new { message = "Um ou mais alojamentos não estão disponíveis." });
        }

        var hotelIds = accommodations.Select(x => x.HotelId).Distinct().ToList();
        if (hotelIds.Count != 1)
        {
            return BadRequest(new { message = "Uma reserva só pode incluir alojamentos do mesmo hotel." });
        }

        var nights = request.CheckOut.DayNumber - request.CheckIn.DayNumber;
        var booking = new Booking
        {
            CustomerId = userId,
            CheckIn = request.CheckIn,
            CheckOut = request.CheckOut,
            Status = BookingStatus.Pending
        };

        foreach (var item in requestedItems)
        {
            var accommodation = accommodations.First(x => x.Id == item.AccommodationId);
            if (item.Quantity <= 0 || item.Quantity > accommodation.Stock)
            {
                return BadRequest(new { message = $"Stock insuficiente para {accommodation.Name}." });
            }

            booking.Items.Add(new BookingItem
            {
                AccommodationId = accommodation.Id,
                Quantity = item.Quantity,
                UnitPrice = accommodation.FinalPrice
            });
        }

        booking.Total = booking.Items.Sum(x => x.UnitPrice * x.Quantity * nights);
        db.Bookings.Add(booking);
        await db.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(nameof(GetMine), new { id = booking.Id }, new { booking.Id, booking.Total, booking.Status });
    }

    [HttpPost("{id:int}/pay")]
    public async Task<IActionResult> Pay(int id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var booking = await db.Bookings
            .Include(item => item.Items)
            .FirstOrDefaultAsync(item => item.Id == id && item.CustomerId == userId, cancellationToken);

        if (booking is null)
        {
            return NotFound();
        }

        if (booking.Status is BookingStatus.Paid)
        {
            return Ok(new { booking.Id, booking.Status });
        }

        if (booking.Status is BookingStatus.Rejected or BookingStatus.Cancelled)
        {
            return BadRequest(new { message = "Esta reserva não pode ser paga." });
        }

        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
        if (!booking.StockUpdated)
        {
            foreach (var item in booking.Items)
            {
                var accommodation = await db.Accommodations
                    .FirstOrDefaultAsync(value => value.Id == item.AccommodationId, cancellationToken);
                if (accommodation is null || accommodation.Stock < item.Quantity)
                {
                    return BadRequest(new { message = "O stock de um alojamento deixou de estar disponível." });
                }

                accommodation.Stock -= item.Quantity;
            }

            booking.StockUpdated = true;
        }

        booking.Status = BookingStatus.Paid;
        await db.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return Ok(new { booking.Id, booking.Status, booking.Total });
    }
}
