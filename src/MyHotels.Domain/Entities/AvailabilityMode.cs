namespace MyHotels.Domain.Entities;

public sealed class AvailabilityMode
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<Accommodation> Accommodations { get; set; } = [];
}
