namespace MyHotels.Shared.Data.DTO;

public sealed class AccommodationDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Capacity { get; set; }
    public int Stock { get; set; }
    public decimal FinalPrice { get; set; }
    public string? ImagePath { get; set; }
    public bool IsAvailableForBooking { get; set; }
    public int HotelId { get; set; }
    public string HotelName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int Stars { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string AvailabilityMode { get; set; } = string.Empty;
}
