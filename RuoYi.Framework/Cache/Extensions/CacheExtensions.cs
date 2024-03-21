using Microsoft.Extensions.DependencyInjection;
using RuoYi.Framework.Cache.Memory;
using RuoYi.Framework.Cache.Redis;
using StackExchange.Redis;

namespace RuoYi.Framework.Cache;

public static class CacheExtensions
{
    public static IServiceCollection AddCache(this IServiceCollection services)
    {
        var cacheConfig = App.GetConfig<CacheConfig>("CacheConfig");
        if (cacheConfig.CacheType == Enums.CacheType.Memory)
        {
            services.AddTransient<ICache, MemoryCache>();
            return AddMemoryCache(services);
        }
        else
        {
            services.AddTransient<ICache, RedisCache>();
            return AddStackExchangeRedisCache(services);
        }
    }

    private static IServiceCollection AddMemoryCache(this IServiceCollection services)
    {
        services.AddDistributedMemoryCache();
        return services;
    }

    private static IServiceCollection AddStackExchangeRedisCache(this IServiceCollection services)
    {
        var cacheConfig = App.GetConfig<CacheConfig>("CacheConfig");
        var redisConfig = cacheConfig.RedisConfig;
        services.AddStackExchangeRedisCache(options =>
        {
            // 连接字符串
            options.Configuration = $"{redisConfig.Host}:{redisConfig.Port},defaultDatabase={redisConfig.Database}, password={redisConfig.Password}";
            // 键名前缀
            options.InstanceName = redisConfig.InstanceName;

            //options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions
            //{
            //    DefaultDatabase = redisConfig.Database,
            //    Password = redisConfig.Password,
            //    EndPoints = { $"{redisConfig.Host}:{redisConfig.Port}" }
            //    //ConnectTimeout = 5000,//设置建立连接到Redis服务器的超时时间为5000毫秒
            //    //SyncTimeout = 5000,//设置对Redis服务器进行同步操作的超时时间为5000毫秒
            //    //Ssl = false,//设置启用SSL安全加密传输Redis数据
            //    //ConnectRetry = 3 // 重试次数
            //};
        });

        // IDistributedCache 没有 模糊匹配 key 的方法, 此处注入 IConnectionMultiplexer 来 实现 IDistributedCache 中不存在的功能
        services.AddSingleton<IConnectionMultiplexer>(sp =>
             ConnectionMultiplexer.Connect(new ConfigurationOptions
             {
                 EndPoints = { $"{redisConfig.Host}:{redisConfig.Port}" },
                 DefaultDatabase = redisConfig.Database,
                 Password = redisConfig.Password,
                 AbortOnConnectFail = false
             })
        );

        return services;
    }
}