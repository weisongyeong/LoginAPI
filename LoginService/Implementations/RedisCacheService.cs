using LoginService.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace LoginService.Implementations;

public class RedisCacheService(
    IConnectionMultiplexer redis
    ) : IRedisCacheService
{
    public async Task SetCacheValueAsync<T>(string key, T value, TimeSpan expiration)
    {
        var db = redis.GetDatabase();
        var json = JsonSerializer.Serialize(value);
        await db.StringSetAsync(key, json, expiration);
    }

    public async Task<T> GetCacheValueAsync<T>(string key)
    {
        var db = redis.GetDatabase();
        var json = await db.StringGetAsync(key);
        return json.HasValue ? JsonSerializer.Deserialize<T>(json) : default;
    }
}