using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyHotels.Infrastructure.Data;

namespace MyHotels.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AccommodationsController(ApplicationDbContext db) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetVisible(
        [FromQuery] int? hotelId,
        [FromQuery] int? categoryId,
        CancellationToken cancellationToken)
    {
        var query = db.Accommodations
            .AsNoTracking()
            .Include(x => x.Hotel)
            .Include(x => x.Category)
            .Include(x => x.AvailabilityMode)
            .Where(x => x.IsActive && x.IsListed && x.Hotel != null && x.Hotel.IsActive);

        if (hotelId is > 0)
        {
            query = query.Where(x => x.HotelId == hotelId);
        }

        if (categoryId is > 0)
        {
            query = query.Where(x => x.CategoryId == categoryId);
        }

        var items = await query
            .OrderBy(x => x.Name)
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.Description,
                x.Capacity,
                x.Stock,
                x.FinalPrice,
                x.ImagePath,
                x.IsAvailableForBooking,
                HotelId = x.HotelId,
                HotelName = x.Hotel!.Name,
                City = x.Hotel.City,
                Stars = x.Hotel.Stars,
                CategoryId = x.CategoryId,
                CategoryName = x.Category!.Name,
                AvailabilityMode = x.AvailabilityMode!.Name
            })
            .ToListAsync(cancellationToken);

        return Ok(items);
    }

    [HttpGet("featured")]
    [AllowAnonymous]
    public async Task<IActionResult> GetFeatured(CancellationToken cancellationToken)
    {
        var candidates = await db.Accommodations
            .AsNoTracking()
            .Include(x => x.Hotel)
            .Where(x => x.IsActive && x.IsListed && x.IsAvailableForBooking && x.Hotel != null && x.Hotel.IsActive)
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.Description,
                x.FinalPrice,
                x.ImagePath,
                HotelName = x.Hotel!.Name,
                City = x.Hotel.City
            })
            .ToListAsync(cancellationToken);

        if (candidates.Count == 0)
        {
            return NotFound();
        }

        var featured = candidates[Random.Shared.Next(candidates.Count)];
        return Ok(featured);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var item = await db.Accommodations
            .AsNoTracking()
            .Include(x => x.Hotel)
            .Include(x => x.Category)
            .Include(x => x.AvailabilityMode)
            .Where(x => x.Id == id && x.IsActive && x.IsListed)
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.Description,
                x.Capacity,
                x.Stock,
                x.FinalPrice,
                x.ImagePath,
                x.IsAvailableForBooking,
                HotelId = x.HotelId,
                HotelName = x.Hotel!.Name,
                City = x.Hotel.City,
                Stars = x.Hotel.Stars,
                CategoryName = x.Category!.Name,
                AvailabilityMode = x.AvailabilityMode!.Name
            })
            .FirstOrDefaultAsync(cancellationToken);

        return item is null ? NotFound() : Ok(item);
    }
}
