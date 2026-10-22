namespace MyHotels.Domain.Entities;

public sealed class BookingItem
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public int AccommodationId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public Booking? Booking { get; set; }
    public Accommodation? Accommodation { get; set; }
}
