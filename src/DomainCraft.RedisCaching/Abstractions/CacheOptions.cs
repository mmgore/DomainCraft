namespace DomainCraft.RedisCaching.Abstractions;

public class CacheOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public int Database { get; set; } = -1; 
}