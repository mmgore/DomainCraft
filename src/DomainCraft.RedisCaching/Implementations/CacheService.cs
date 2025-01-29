using DomainCraft.RedisCaching.Abstractions;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace DomainCraft.RedisCaching.Implementations;

public class CacheService(IConnectionMultiplexer redis, int database = -1) : ICacheService
{
    private readonly IDatabase _database = redis.GetDatabase(database);

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _database.StringGetAsync(key);
        return value.IsNullOrEmpty ? default : JsonConvert.DeserializeObject<T>(value!);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        var serializedValue = JsonConvert.SerializeObject(value);
        await _database.StringSetAsync(key, serializedValue, expiration);
    }

    public async Task RemoveAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    }
}