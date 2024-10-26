using Microsoft.Extensions.Caching.Memory;

namespace TaskManager.Application.Modules.Email.Code.Storage;

public sealed class CacheCodeStorage(IMemoryCache memoryCache) : ICodeStorage
{
    private readonly IMemoryCache _memoryCache = memoryCache;

    public void Set(string code, string email)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(7)
        };

        _memoryCache.Set(code, email, options);
    }

    public bool TryGetValue(string code, out string? email) => _memoryCache.TryGetValue(code, out email);

    public void Remove(string code) => _memoryCache.Remove(code);
}
