namespace MyHotels.Shared.Data.Services;

public interface ITokenStore
{
    ValueTask<string?> GetAsync(CancellationToken cancellationToken = default);
    ValueTask SetAsync(string token, CancellationToken cancellationToken = default);
    ValueTask ClearAsync(CancellationToken cancellationToken = default);
}
