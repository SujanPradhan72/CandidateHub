using CandidateHub.Data.Caching;
using Microsoft.Extensions.Caching.Memory;

namespace CandidateHub.Tests.Services;

public class LocalCachingServiceTest
{
    private readonly LocalCachingService _cachingService = new(new MemoryCache(new MemoryCacheOptions()));

    [Fact]
    public async Task GetValueAsync_ShouldReturnCachedValue_WhenKeyExists()
    {
        // Arrange
        string key = "testKey";
        string expectedValue = "testValue";
        await _cachingService.SetValueAsync(key, expectedValue);

        // Act
        var actualValue = await _cachingService.GetValueAsync<string>(key);

        // Assert
        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public async Task GetValueAsync_ShouldReturnNull_WhenKeyDoesNotExist()
    {
        // Arrange
        string key = "nonExistentKey";

        // Act
        var actualValue = await _cachingService.GetValueAsync<string>(key);

        // Assert
        Assert.Null(actualValue);
    }

    [Fact]
    public async Task SetValueAsync_ShouldCacheValueWithExpiration()
    {
        // Arrange
        string key = "testKey";
        string value = "testValue";
        TimeSpan expiration = TimeSpan.FromMinutes(1);

        // Act
        await _cachingService.SetValueAsync(key, value, expiration);

        // Assert
        var cachedValue = await _cachingService.GetValueAsync<string>(key);
        Assert.Equal(value, cachedValue);
    }
}