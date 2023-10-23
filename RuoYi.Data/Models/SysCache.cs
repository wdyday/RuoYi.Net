namespace RuoYi.Data.Models;

public class SysCache
{
    /** 缓存名称 */
    public string? CacheName { get; set; } = string.Empty;

    /** 缓存键名 */
    public string? CacheKey { get; set; } = string.Empty;

    /** 缓存内容 */
    public string? CacheValue { get; set; } = string.Empty;

    /** 备注 */
    public string? Remark { get; set; } = string.Empty;

    public SysCache() { }

    public SysCache(string cacheName, string remark)
    {
        this.CacheName = cacheName;
        this.Remark = remark;
    }

    public SysCache(string cacheName, string cacheKey, string cacheValue)
    {
        this.CacheName = cacheName;
        this.CacheKey = cacheKey;
        this.CacheValue = cacheValue;
    }
}
