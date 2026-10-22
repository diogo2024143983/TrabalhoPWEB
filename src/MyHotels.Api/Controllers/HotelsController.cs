using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyHotels.Infrastructure.Data;

namespace MyHotels.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class HotelsController(ApplicationDbContext db) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetActiveHotels(CancellationToken cancellationToken)
    {
        var hotels = await db.Hotels
            .AsNoTracking()
            .Where(hotel => hotel.IsActive)
            .OrderBy(hotel => hotel.Name)
            .Select(hotel => new
            {
                hotel.Id,
                hotel.Name,
                hotel.City,
                hotel.Description,
                hotel.Stars,
                hotel.ImagePath
            })
            .ToListAsync(cancellationToken);

        return Ok(hotels);
    }
}
