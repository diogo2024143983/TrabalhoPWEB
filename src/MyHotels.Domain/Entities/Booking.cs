namespace MyHotels.Domain.Entities;

public sealed class Booking
{
    public int Id { get; set; }
    public required string CustomerId { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateOnly CheckIn { get; set; }
    public DateOnly CheckOut { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public decimal Total { get; set; }
    public bool StockUpdated { get; set; }
    public bool DocumentsExpedited { get; set; }
    public ICollection<BookingItem> Items { get; set; } = [];
}
