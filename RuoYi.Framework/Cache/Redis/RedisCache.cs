using Microsoft.Extensions.Caching.Distributed;
using RuoYi.Framework.Cache.Options;
using RuoYi.Framework.JsonSerialization;
using RuoYi.Framework.Logging;
using RuoYi.Framework.Utils;
using StackExchange.Redis;
using System.Text;

namespace RuoYi.Framework.Cache.Redis
{
    public class RedisCache : ICache
    {
        private static readonly DistributedCacheEntryOptions DefaultOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(36500));

        private readonly IDistributedCache _cache;
        private readonly IConnectionMultiplexer _multiplexer;
        private readonly IDatabase _database;
        private readonly IServer _server;
        private readonly RedisConfig _redisConfig;

        public RedisCache(IDistributedCache cache, IConnectionMultiplexer multiplexer)
        {
            _cache = cache;
            _multiplexer = multiplexer;
            _database = _multiplexer.GetDatabase();
            _server = _multiplexer.GetServer(_multiplexer.GetEndPoints()[0]);

            _redisConfig = App.GetConfig<RedisConfig>("CacheConfig:RedisConfig");
        }

        #region String
        public string? GetString(string key)
        {
            return _cache.GetString(key);
        }

        public void SetString(string key, string value)
        {
            _cache.SetString(key, value, DefaultOptions);
        }

        public void SetString(string key, string value, long minutes)
        {
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(minutes));
            _cache.SetString(key, value, options);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public async Task<string?> GetStringAsync(string key)
        {
            return await _cache.GetStringAsync(key, default);
        }

        public async Task SetStringAsync(string key, string value)
        {
            await _cache.SetStringAsync(key, value, DefaultOptions, default);
        }

        public async Task SetStringAsync(string key, string value, long minutes)
        {
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(minutes));
            await _cache.SetStringAsync(key, value, options, default);
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
        #endregion

        #region DB(IConnectionMultiplexer)

        public IEnumerable<string> GetDbKeys(string pattern, int pageSize = 1000)
        {
            var keys = GetKeys(pattern, pageSize);
            return ToStringKeys(keys);
        }

        /// <summary>
        /// 按名字删除缓存
        /// </summary>
        public void RemoveByPattern(string cacheName)
        {
            var redisKeys = GetKeys(cacheName);
            Remove(redisKeys);
        }

        public async Task<Dictionary<string, string>> GetDbInfoAsync(params object[] args)
        {
            var info = (await ExecuteAsync("INFO", args)).ToString();

            return string.IsNullOrEmpty(info)
                ? new Dictionary<string, string>()
                : ParseInfo(info);
        }

        /// <summary>
        /// 当前db中 key数量
        /// </summary>
        /// <returns></returns>
        public async Task<long> GetDbSize()
        {
            var dbsize = (await ExecuteAsync("dbsize")).ToString();
            return Convert.ToInt64(dbsize);
        }

        #region private

        private IEnumerable<RedisKey> GetKeys(string pattern, int pageSize = 1000)
        {
            if (!pattern.StartsWith('*'))
            {
                pattern = _redisConfig.InstanceName + pattern;
            }
            return _server.Keys(pattern: pattern, pageSize: pageSize);
        }

        private IEnumerable<string> ToStringKeys(IEnumerable<RedisKey> keys)
        {
            return keys.Select(k =>
            {
                var key = k.ToString();
                // 去掉开头的 ruoyi_net:
                return key.StartsWith(_redisConfig.InstanceName) ? StringUtils.StripStart(key, _redisConfig.InstanceName) : key;
            });
        }

        private long Remove(IEnumerable<RedisKey> keys)
        {
            return _database.KeyDelete(keys.ToArray());
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        private async Task<RedisResult> ExecuteAsync(string command, params object[] args)
        {
            return await _database.ExecuteAsync(command, args);
        }

        private Dictionary<string, string> ParseInfo(string info)
        {
            // Call Parse Categorized Info to cut back on duplicated code.
            var data = ParseCategorizedInfo(info);

            // Return a dictionary of the Info Key and Info value

            var result = new Dictionary<string, string>();

            for (var i = 0; i < data.Length; i++)
            {
                var x = data[i];

                result.Add(x.Key, x.InfoValue);
            }

            return result;
        }

        private InfoDetail[] ParseCategorizedInfo(string info)
        {
            var data = new List<InfoDetail>();
            var category = string.Empty;

            var lines = info.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines.Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                if (line[0] == '#')
                {
                    category = line.Replace("#", string.Empty).Trim();
                    continue;
                }

                var idx = line.IndexOf(':');

                if (idx > 0)
                {
                    var key = line.Substring(0, idx);
                    var infoValue = line.Substring(idx + 1).Trim();

                    data.Add(new(category, key, infoValue));
                }
            }

            return data.ToArray();
        }
        #endregion

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
                Log.Error("RedisCache Get error", e);
                return default!;
            }
        }

        public void Set<T>(string key, T value)
        {
            _cache.SetString(key, JSON.Serialize(value!), DefaultOptions);
        }

        public void Set<T>(string key, T value, long minutes)
        {
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(minutes));
            _cache.SetString(key, JSON.Serialize(value!), options);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            try
            {
                var valueString = await _cache.GetStringAsync(key, default);
                if (string.IsNullOrEmpty(valueString))
                {
                    return default!;
                }

                return JSON.Deserialize<T>(valueString);
            }
            catch (Exception e)
            {
                Log.Error("RedisCache GetAsync error", e);
                return default!;
            }
        }
        public async Task SetAsync<T>(string key, T value)
        {
            await _cache.SetStringAsync(key, JSON.Serialize(value!), DefaultOptions, default);
        }

        public async Task SetAsync<T>(string key, T value, long minutes)
        {
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(minutes));
            await _cache.SetStringAsync(key, JSON.Serialize(value!), options, default);
        }
    }
    #endregion
}
