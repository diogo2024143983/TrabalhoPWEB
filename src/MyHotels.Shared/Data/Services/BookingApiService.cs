using System.Net.Http.Json;
using MyHotels.Shared.Data.DTO;

namespace MyHotels.Shared.Data.Services;

public sealed class BookingApiService(HttpClient httpClient)
{
    public async Task<AuthResponse?> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/Auth/login", request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<AuthResponse>(cancellationToken);
    }

    public async Task<bool> RegisterAsync(
        RegisterCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/Auth/register-cliente", request, cancellationToken);
        return response.IsSuccessStatusCode;
    }

    public async Task<int?> CreateBookingAsync(
        CreateBookingRequest request,
        CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("api/Bookings", request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var result = await response.Content.ReadFromJsonAsync<CreateBookingResponse>(cancellationToken);
        return result?.Id;
    }

    public async Task<bool> PayAsync(int bookingId, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsync($"api/Bookings/{bookingId}/pay", null, cancellationToken);
        return response.IsSuccessStatusCode;
    }

    public async Task<IReadOnlyList<BookingSummaryDto>> GetMineAsync(
        CancellationToken cancellationToken = default)
    {
        var result = await httpClient.GetFromJsonAsync<List<BookingSummaryDto>>(
            "api/Bookings/mine",
            cancellationToken);
        return result ?? [];
    }

    private sealed record CreateBookingResponse(int Id);
}
