using Microsoft.AspNetCore.Identity;
using MyHotels.Domain.Identity;

namespace MyHotels.Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Nif { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; } = true;
    public CustomerStatus CustomerStatus { get; set; } = CustomerStatus.Pending;
}
