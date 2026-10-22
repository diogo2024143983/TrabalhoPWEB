using MyHotels.Shared.Data.DTO;

namespace MyHotels.Shared.Data.Services;

public sealed class CartState
{
    private readonly List<CartItem> _items = [];

    public IReadOnlyList<CartItem> Items => _items;
    public event Action? Changed;

    public void Add(AccommodationDto accommodation, int quantity = 1)
    {
        if (!accommodation.IsAvailableForBooking || quantity <= 0)
        {
            return;
        }

        if (_items.Count > 0 && _items[0].HotelId != accommodation.HotelId)
        {
            _items.Clear();
        }

        var existing = _items.FirstOrDefault(x => x.AccommodationId == accommodation.Id);
        if (existing is null)
        {
            _items.Add(new CartItem(
                accommodation.Id,
                accommodation.HotelId,
                accommodation.Name,
                accommodation.HotelName,
                accommodation.FinalPrice,
                quantity));
        }
        else
        {
            existing.Quantity += quantity;
        }

        Changed?.Invoke();
    }

    public void Clear()
    {
        _items.Clear();
        Changed?.Invoke();
    }

    public void SetQuantity(int accommodationId, int quantity)
    {
        var item = _items.FirstOrDefault(value => value.AccommodationId == accommodationId);
        if (item is null) return;
        if (quantity <= 0)
        {
            _items.Remove(item);
        }
        else
        {
            item.Quantity = quantity;
        }

        Changed?.Invoke();
    }

    public sealed class CartItem(
        int accommodationId,
        int hotelId,
        string name,
        string hotelName,
        decimal unitPrice,
        int quantity)
    {
        public int AccommodationId { get; } = accommodationId;
        public int HotelId { get; } = hotelId;
        public string Name { get; } = name;
        public string HotelName { get; } = hotelName;
        public decimal UnitPrice { get; } = unitPrice;
        public int Quantity { get; set; } = quantity;
    }
}
