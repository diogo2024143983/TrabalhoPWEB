using System.Net.Http.Json;
using MyHotels.Shared.Data.DTO;
using MyHotels.Shared.Data.Interfaces;

namespace MyHotels.Shared.Data.Services;

public sealed class CatalogApiService(HttpClient httpClient) : ICatalogApi
{
    public async Task<IReadOnlyList<HotelDto>> GetHotelsAsync(CancellationToken cancellationToken = default)
    {
        var result = await httpClient.GetFromJsonAsync<List<HotelDto>>("api/Hotels", cancellationToken);
        return result ?? [];
    }

    public async Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var result = await httpClient.GetFromJsonAsync<List<CategoryDto>>("api/Categories", cancellationToken);
        return result ?? [];
    }

    public async Task<IReadOnlyList<AccommodationDto>> GetAccommodationsAsync(
        int? hotelId = null,
        int? categoryId = null,
        CancellationToken cancellationToken = default)
    {
        var query = "api/Accommodations";
        var parts = new List<string>();
        if (hotelId is > 0) parts.Add($"hotelId={hotelId}");
        if (categoryId is > 0) parts.Add($"categoryId={categoryId}");
        if (parts.Count > 0) query += "?" + string.Join("&", parts);

        var result = await httpClient.GetFromJsonAsync<List<AccommodationDto>>(query, cancellationToken);
        return result ?? [];
    }

    public async Task<FeaturedAccommodationDto?> GetFeaturedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await httpClient.GetFromJsonAsync<FeaturedAccommodationDto>("api/Accommodations/featured", cancellationToken);
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<AccommodationDto?> GetAccommodationAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await httpClient.GetFromJsonAsync<AccommodationDto>($"api/Accommodations/{id}", cancellationToken);
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }
}
