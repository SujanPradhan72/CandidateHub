namespace CandidateHub.Data.Caching;

public interface ICachingService
{
    Task<T?> GetValueAsync<T>(string key);
    Task SetValueAsync<T>(string key, T data, TimeSpan? expiration = null);
}