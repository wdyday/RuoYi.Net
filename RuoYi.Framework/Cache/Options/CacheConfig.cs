using RuoYi.Framework.Cache.Enums;
using RuoYi.Framework.Cache.Options;

namespace RuoYi.Framework.Cache.Redis;

public class CacheConfig
{
    public CacheType CacheType { get; set; }
    
    public RedisConfig? RedisConfig { get; set; }
}
