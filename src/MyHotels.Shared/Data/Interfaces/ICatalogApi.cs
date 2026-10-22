using MyHotels.Shared.Data.DTO;

namespace MyHotels.Shared.Data.Interfaces;

public interface ICatalogApi
{
    Task<IReadOnlyList<HotelDto>> GetHotelsAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AccommodationDto>> GetAccommodationsAsync(int? hotelId = null, int? categoryId = null, CancellationToken cancellationToken = default);
    Task<FeaturedAccommodationDto?> GetFeaturedAsync(CancellationToken cancellationToken = default);
    Task<AccommodationDto?> GetAccommodationAsync(int id, CancellationToken cancellationToken = default);
}
