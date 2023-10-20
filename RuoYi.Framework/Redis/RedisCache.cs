using Furion;
using Furion.DependencyInjection;
using Furion.JsonSerialization;
using Furion.Logging;
using Microsoft.Extensions.Caching.Distributed;
using RuoYi.Framework.Utils;
using StackExchange.Redis;
using System.Text;

namespace RuoYi.Framework.Redis
{
    public class RedisCache : ITransient
    {
        private static readonly DistributedCacheEntryOptions DefaultOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

        private readonly IDistributedCache _cache;
        private readonly IConnectionMultiplexer _multiplexer;
        private readonly IDatabaseAsync _database;
        private readonly IServer _server;
        private readonly RedisConfig _redisConfig;

        public RedisCache(IDistributedCache cache, IConnectionMultiplexer multiplexer)
        {
            _cache = cache;
            _multiplexer = multiplexer;
            _database = _multiplexer.GetDatabase();
            _server = _multiplexer.GetServer(_multiplexer.GetEndPoints()[0]);

            _redisConfig = App.GetConfig<RedisConfig>("RedisConfig");
        }

        #region IDistributedCache 自带方法
        public string? GetString(string key)
        {
            return _cache.GetString(key);
        }

        public async Task<string?> GetStringAsync(string key, CancellationToken token = default(CancellationToken))
        {
            return await _cache.GetStringAsync(key, token);
        }

        public void SetString(string key, string value)
        {
            _cache.SetString(key, value, DefaultOptions);
        }

        public void SetString(string key, string value, DistributedCacheEntryOptions options)
        {
            _cache.SetString(key, value, options);
        }

        public void SetString(string key, string value, long minutes)
        {
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(minutes));
            _cache.SetString(key, value, options);
        }

        public async Task SetStringAsync(string key, string value, CancellationToken token = default(CancellationToken))
        {
            await _cache.SetStringAsync(key, value, DefaultOptions, token);
        }

        public async Task SetStringAsync(string key, string value, long minutes, CancellationToken token = default(CancellationToken))
        {
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(minutes));
            await _cache.SetStringAsync(key, value, options, token);
        }

        public async Task SetStringAsync(string key, string value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
        {
            await _cache.SetStringAsync(key, value, options, token);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
        #endregion

        #region IConnectionMultiplexer

        //public List<string> GetKeys(string pattern, int pageSize = 1000)
        //{
        //    var keys = _server.Keys(pattern: pattern, pageSize: pageSize)
        //        .Select(k => k.ToString())
        //        .ToList();
        //    return keys;
        //}

        public IEnumerable<RedisKey> GetKeys(string pattern, int pageSize = 1000)
        {
            if (!pattern.StartsWith('*'))
            {
                pattern = _redisConfig.InstanceName + pattern;
            }
            return _server.Keys(pattern: pattern, pageSize: pageSize);
        }

        public IEnumerable<string> GetStringKeys(string pattern, int pageSize = 1000)
        {
            var keys = GetKeys(pattern, pageSize);
            return ToStringKeys(keys);
        }

        public long Remove(IEnumerable<RedisKey> keys)
        {
            return _database.KeyDeleteAsync(keys.ToArray()).GetAwaiter().GetResult();
        }

        //public IAsyncEnumerable<RedisKey> GetKeysAsync(string pattern, int pageSize = 1000)
        //{
        //    if (!pattern.StartsWith('*'))
        //    {
        //        pattern = _redisConfig.InstanceName + pattern;
        //    }
        //    return _server.KeysAsync(pattern: pattern, pageSize: pageSize);
        //}

        public async Task<long> RemoveAsync(IEnumerable<RedisKey> keys)
        {
            return await _database.KeyDeleteAsync(keys.ToArray());
        }

        public IEnumerable<string> ToStringKeys(IEnumerable<RedisKey> keys)
        {
            return keys.Select(k =>
            {
                var key = k.ToString();
                // 去掉开头的 ruoyi_net:
                return key.StartsWith(_redisConfig.InstanceName) ? StringUtils.StripStart(key, _redisConfig.InstanceName) : key;
            });
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command">命令</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public async Task<RedisResult> ExecuteAsync(string command, params object[] args)
        {
            return await _database.ExecuteAsync(command, args);
        }

        public async Task<Dictionary<string, string>> GetRedisInfoAsync()
        {
            //var info = (await _database.ScriptEvaluateAsync("return redis.call('INFO')").ConfigureAwait(false)).ToString();
            var info = (await ExecuteAsync("INFO")).ToString();

            return string.IsNullOrEmpty(info)
                ? new Dictionary<string, string>()
                : ParseInfo(info);
        }

        public async Task<Dictionary<string, string>> GetRedisInfoAsync(params object[] args)
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

        public T Get<T>(string key)
        {
            try
            {
                var valueString = _cache.GetString(key);

                if (string.IsNullOrEmpty(valueString))
                {
                    return default(T)!;
                }
                return JSON.Deserialize<T>(valueString);
            }
            catch (Exception e)
            {
                Log.Error("RedisCache Get error", e);
                return default(T)!;
            }
        }

        public async Task<T> GetAsync<T>(string key, CancellationToken token = default(CancellationToken))
        {
            try
            {
                var valueString = await _cache.GetStringAsync(key, token);
                if (string.IsNullOrEmpty(valueString))
                {
                    return default(T)!;
                }

                return JSON.Deserialize<T>(valueString);
            }
            catch (Exception e)
            {
                Log.Error("RedisCache GetAsync error", e);
                return default(T)!;
            }
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            var valueString = _cache.GetString(key);
            if (!string.IsNullOrEmpty(valueString))
            {
                value = JSON.Deserialize<T>(valueString);
                return true;
            }
            value = default(T)!;
            return false;
        }

        public void Set<T>(string key, T value)
        {
            _cache.SetString(key, JSON.Serialize(value), DefaultOptions);
        }

        public void Set<T>(string key, T value, DistributedCacheEntryOptions options)
        {
            _cache.SetString(key, JSON.Serialize(value), options);
        }

        public void Set<T>(string key, T value, long minutes)
        {
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(minutes));
            _cache.SetString(key, JSON.Serialize(value), options);
        }

        public async Task SetAsync<T>(string key, T value, CancellationToken token = default(CancellationToken))
        {
            await _cache.SetStringAsync(key, JSON.Serialize(value), DefaultOptions, token);
        }

        public async Task SetAsync<T>(string key, T value, long minutes, CancellationToken token = default(CancellationToken))
        {
            var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(minutes));
            await _cache.SetStringAsync(key, JSON.Serialize(value), options, token);
        }

        public async Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken))
        {
            await _cache.SetStringAsync(key, JSON.Serialize(value), options, token);
        }

        public T GetOrCreate<T>(string key, Func<T> factory)
        {
            if (!this.TryGetValue(key, out T obj))
            {
                obj = factory();
                this.Set(key, obj);
            }
            return (T)obj;
        }

        public T GetOrCreate<T>(string key, Func<T> factory, DistributedCacheEntryOptions options)
        {
            if (!this.TryGetValue(key, out T obj))
            {
                obj = factory();
                this.Set(key, obj, options);
            }
            return (T)obj;
        }

        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory)
        {
            if (!this.TryGetValue(key, out T obj))
            {
                obj = await factory();
                await this.SetAsync(key, obj);
            }
            return (T)obj;
        }

        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, DistributedCacheEntryOptions options)
        {
            if (!this.TryGetValue(key, out T obj))
            {
                obj = await factory();
                await this.SetAsync(key, obj, options);
            }
            return (T)obj;
        }
    }
}
