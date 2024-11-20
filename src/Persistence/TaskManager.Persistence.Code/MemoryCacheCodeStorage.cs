using Microsoft.Extensions.Caching.Memory;
using TaskManager.Application.Common.Code;

namespace TaskManager.Persistence.Code;

/// <summary>
/// 
/// </summary>
/// <param name="memoryCache">=</param>
public sealed class MemoryCacheCodeStorage(IMemoryCache memoryCache) : ICodeStorage
{
    private readonly IMemoryCache _memoryCache = memoryCache;

    public void Add(string code, string email)
    {
        _memoryCache.Set(code, email);
    }

    public string? GetEmail(string code)
    {
        _memoryCache.TryGetValue(code, out string? email);

        return email;
    }

    public void Remove(string code) => _memoryCache.Remove(code);
}
