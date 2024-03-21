using Microsoft.Extensions.Caching.Distributed;
using RuoYi.Framework.JsonSerialization;
using RuoYi.Framework.Logging;
using System.Reflection;
using System.Text;

namespace RuoYi.Framework.Cache.Memory;

public class MemoryCache : ICache
{
    private readonly IDistributedCache _cache;

    public MemoryCache(IDistributedCache cache)
    {
        _cache = cache;
    }

    #region String
    public string? GetString(string key)
    {
        return _cache.GetString(key);
    }
    public void SetString(string key, string value)
    {
        this.SetCache(key, value);
    }
    public void SetString(string key, string value, long minutes)
    {
        this.SetCache(key, value, minutes);
    }
    public void Remove(string key)
    {
        _cache.Remove(key);
    }

    public async Task<string?> GetStringAsync(string key)
    {
        return await this.GetCacheAsync(key);
    }
    public async Task SetStringAsync(string key, string value)
    {
        await this.SetCacheAsync(key, value);
    }
    public async Task SetStringAsync(string key, string value, long minutes)
    {
        await this.SetCacheAsync(key, value, minutes);
    }
    public async Task RemoveAsync(string key)
    {
        await this.RemoveCacheAsync(key);
    }
    #endregion

    #region DB
    public async Task<Dictionary<string, string>> GetDbInfoAsync(params object[] args)
    {
        await Task.Delay(0);
        return new Dictionary<string, string>();
    }

    public IEnumerable<string> GetDbKeys(string pattern, int pageSize = 1000)
    {
        return GetKeys(pattern);
    }

    public async Task<long> GetDbSize()
    {
        await Task.Delay(0);
        return 0;
    }

    public void RemoveByPattern(string cacheName)
    {
        var keys = GetKeys(cacheName);
        this.RemoveCaches(keys);
    }
    #endregion

    #region 泛型
    public T Get<T>(string key)
    {
        try
        {
            var valueString = _cache.GetString(key);

            if (string.IsNullOrEmpty(valueString))
            {
                return default!;
            }
            return JSON.Deserialize<T>(valueString);
        }
        catch (Exception e)
        {
            Log.Error("MemoryCache Get error", e);
            return default!;
        }
    }

    public void Set<T>(string key, T value)
    {
        this.SetCache(key, JSON.Serialize(value!));
    }

    public void Set<T>(string key, T value, long minutes)
    {
        this.SetCache(key, JSON.Serialize(value!), minutes);
    }

    public async Task<T> GetAsync<T>(string key)
    {
        try
        {
            var valueString = await this.GetCacheAsync(key);
            if (string.IsNullOrEmpty(valueString))
            {
                return default!;
            }

            return JSON.Deserialize<T>(valueString);
        }
        catch (Exception e)
        {
            Log.Error("MemoryCache GetAsync error", e);
            return default!;
        }
    }

    public async Task SetAsync<T>(string key, T value)
    {
        await this.SetCacheAsync(key, JSON.Serialize(value!));
    }

    public async Task SetAsync<T>(string key, T value, long minutes)
    {
        await this.SetCacheAsync(key, JSON.Serialize(value!), minutes);
    }
    #endregion

    #region Private

    private void SetCache(string key, string value, long? minutes = default)
    {
        if (minutes.HasValue)
        {
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(minutes.Value));

            _cache.SetString(key, value, options);
        }
        else
        {
            _cache.SetString(key, value);
        }
    }

    private void RemoveCaches(IEnumerable<string> keys)
    {
        foreach (var key in keys)
        {
            _cache.Remove(key);
        }
    }

    #region async
    private async Task<string?> GetCacheAsync(string key)
    {
        return await _cache.GetStringAsync(key, default);
    }

    private async Task SetCacheAsync(string key, string value, long? minutes = default)
    {
        if (minutes.HasValue)
        {
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(minutes.Value));

            await _cache.SetStringAsync(key, value, options);
        }
        else
        {
            await _cache.SetStringAsync(key, value);
        }
    }

    private async Task RemoveCacheAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }

    private List<string> GetKeys(string pattern, int pageSize = 1000)
    {
        // 全部缓存key
        var keys = this.GetAllCacheKeys();

        if (pattern.StartsWith('*') && pattern.EndsWith('*'))
        {
            return keys.Where(k => k.Contains(pattern.TrimStart('*').TrimEnd('*'))).Take(pageSize).ToList();
        }
        else if (pattern.StartsWith('*'))
        {
            return keys.Where(k => k.EndsWith(pattern.TrimStart('*'))).Take(pageSize).ToList();
        }
        else if (pattern.EndsWith('*'))
        {
            return keys.Where(k => k.StartsWith(pattern.TrimEnd('*'))).Take(pageSize).ToList();
        }
        else
        {
            return keys.Where(k => k.Equals(pattern)).Take(pageSize).ToList();
        }
    }

    /* 获取 全部缓存key
     * https://www.cnblogs.com/sflwf/articles/17970233
     */
    private List<string> GetAllCacheKeys()
    {
        var keys = new List<string>();

        const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
        var memCache = _cache.GetType().GetField("_memCache", flags)!.GetValue(_cache);
        var coherentState = memCache!.GetType().GetField("_coherentState", flags)!.GetValue(memCache);
        var entries = coherentState!.GetType().GetField("_entries", flags)!.GetValue(coherentState);

        if (entries is not IDictionary cacheItems) return keys;

        foreach (DictionaryEntry cacheItem in cacheItems!)
        {
            keys.Add(cacheItem.Key.ToString()!);
        }

        return keys;
    }
    #endregion

    #endregion
}
