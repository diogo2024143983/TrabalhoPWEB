using Microsoft.JSInterop;

namespace MyHotels.Shared.Data.Services;

public sealed class BrowserTokenStore(IJSRuntime js) : ITokenStore
{
    private const string StorageKey = "myhotels.jwt";

    public ValueTask<string?> GetAsync(CancellationToken cancellationToken = default) =>
        js.InvokeAsync<string?>("localStorage.getItem", cancellationToken, StorageKey);

    public ValueTask SetAsync(string token, CancellationToken cancellationToken = default) =>
        js.InvokeVoidAsync("localStorage.setItem", cancellationToken, StorageKey, token);

    public ValueTask ClearAsync(CancellationToken cancellationToken = default) =>
        js.InvokeVoidAsync("localStorage.removeItem", cancellationToken, StorageKey);
}
