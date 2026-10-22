using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyHotels.Infrastructure.Data;

namespace MyHotels.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CategoriesController(ApplicationDbContext db) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetActive(CancellationToken cancellationToken)
    {
        var categories = await db.AccommodationCategories
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Name)
            .Select(x => new { x.Id, x.Name, x.Description })
            .ToListAsync(cancellationToken);

        return Ok(categories);
    }
}
