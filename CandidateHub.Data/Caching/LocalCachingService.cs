using Microsoft.Extensions.Caching.Memory;

namespace CandidateHub.Data.Caching;

public class LocalCachingService(IMemoryCache cache) : ICachingService
{
    public async Task<T?> GetValueAsync<T>(string key)
    {
        var data = Task.Run(() => cache.Get<T>(key));
        return await data;
    }

    public Task SetValueAsync<T>(string key, T data, TimeSpan? expiration = null)
    {
        var options = new MemoryCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = expiration
        };
        cache.Set(key, data, options);
        return Task.CompletedTask;
    }
}