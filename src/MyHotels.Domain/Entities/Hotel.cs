namespace MyHotels.Domain.Entities;

public sealed class Hotel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string City { get; set; }
    public string? Description { get; set; }
    public int Stars { get; set; }
    public string? ImagePath { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<Accommodation> Accommodations { get; set; } = [];
}
