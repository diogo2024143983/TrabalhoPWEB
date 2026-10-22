namespace MyHotels.Shared.Data.DTO;

public sealed class HotelDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Stars { get; set; }
    public string? ImagePath { get; set; }
}
