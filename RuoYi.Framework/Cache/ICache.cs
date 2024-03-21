namespace RuoYi.Framework.Cache;

public interface ICache
{
    #region String
    string? GetString(string key);
    void SetString(string key, string value);
    void SetString(string key, string value, long minutes);
    void Remove(string key);

    Task<string?> GetStringAsync(string key);
    Task SetStringAsync(string key, string value);
    Task SetStringAsync(string key, string value, long minutes);
    Task RemoveAsync(string key);
    #endregion

    #region DB
    IEnumerable<string> GetDbKeys(string pattern, int pageSize = 1000);
    Task<Dictionary<string, string>> GetDbInfoAsync(params object[] args);
    Task<long> GetDbSize();
    void RemoveByPattern(string pattern);
    #endregion

    #region 泛型
    T Get<T>(string key);
    void Set<T>(string key, T value);
    void Set<T>(string key, T value, long minutes);

    Task<T> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value);
    Task SetAsync<T>(string key, T value, long minutes);
    #endregion
}
