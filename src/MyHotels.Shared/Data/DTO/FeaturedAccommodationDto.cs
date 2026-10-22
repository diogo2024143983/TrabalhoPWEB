namespace MyHotels.Shared.Data.DTO;

public sealed class FeaturedAccommodationDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal FinalPrice { get; set; }
    public string? ImagePath { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
}
