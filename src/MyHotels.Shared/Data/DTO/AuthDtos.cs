namespace MyHotels.Shared.Data.DTO;

public sealed record LoginRequest(string Email, string Password);

public sealed record RegisterCustomerRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string? Nif,
    string? Address);

public sealed record AuthResponse(
    string Token,
    DateTime ExpiresAtUtc,
    string Email,
    IReadOnlyList<string> Roles);

public sealed record BookingItemRequest(int AccommodationId, int Quantity);

public sealed record CreateBookingRequest(
    DateOnly CheckIn,
    DateOnly CheckOut,
    IReadOnlyList<BookingItemRequest> Items);

public sealed record BookingSummaryDto(
    int Id,
    DateOnly CheckIn,
    DateOnly CheckOut,
    string Status,
    decimal Total,
    DateTime CreatedAtUtc,
    bool DocumentsExpedited,
    IReadOnlyList<BookingItemSummaryDto> Items);

public sealed record BookingItemSummaryDto(
    int AccommodationId,
    int Quantity,
    decimal UnitPrice);
