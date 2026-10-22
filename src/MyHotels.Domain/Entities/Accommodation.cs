namespace MyHotels.Domain.Entities;

public sealed class Accommodation
{
    public int Id { get; set; }
    public int HotelId { get; set; }
    public int CategoryId { get; set; }
    public int AvailabilityModeId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public int Capacity { get; set; }
    public int Stock { get; set; }
    public decimal BasePrice { get; set; }
    public decimal FinalPrice { get; set; }
    public string? ImagePath { get; set; }
    public bool IsListed { get; set; } = true;
    public bool IsAvailableForBooking { get; set; } = true;
    public bool IsActive { get; set; } = true;
    public Hotel? Hotel { get; set; }
    public AccommodationCategory? Category { get; set; }
    public AvailabilityMode? AvailabilityMode { get; set; }
    public ICollection<BookingItem> BookingItems { get; set; } = [];
}
