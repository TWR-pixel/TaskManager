using Microsoft.Extensions.Caching.Memory;

namespace TaskManager.Application.Modules.Email.Code.Storage;

public sealed class CacheCodeStorage(IMemoryCache memoryCache) : ICodeStorage
{
    private readonly IMemoryCache _memoryCache = memoryCache;

    public void Set(string key, string value)
    {
        var options = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(7)
        };

        _memoryCache.Set(key, value, options);
    }

    public bool TryGetValue(string key, out string? value) => _memoryCache.TryGetValue(key, out value);

    public void Remove(string key) => _memoryCache.Remove(key);
}
