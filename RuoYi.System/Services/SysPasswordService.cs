using RuoYi.Common.Constants;
using RuoYi.Common.Utils;
using RuoYi.Framework.Cache;
using RuoYi.Framework.Exceptions;

namespace RuoYi.System.Services;

/// <summary>
/// 登录密码方法
/// </summary>
public class SysPasswordService : ITransient
{
    private readonly ICache _cache;
    private readonly SysLogininforService _sysLogininforService;

    public SysPasswordService(ICache cache, SysLogininforService sysLogininforService)
    {
        _cache = cache;
        _sysLogininforService = sysLogininforService;
    }

    /// <summary>
    /// 登录账户密码错误次数缓存键名
    /// </summary>
    /// <param name="username">用户名</param>
    /// <returns>缓存键key</returns>
    private string GetCacheKey(string username)
    {
        return CacheConstants.PWD_ERR_CNT_KEY + username;
    }

    public void Validate(string username, string password, SysUserDto user)
    {
        var userConfig = RyApp.UserConfig;
        var maxRetryCount = userConfig.MaxRetryCount;
        var lockTime = userConfig.LockTime;

        int retryCount = Convert.ToInt32(_cache.GetString(GetCacheKey(username)) ?? "0");

        if (retryCount >= maxRetryCount)
        {
            var retryLimitExceedMsg = string.Format(MessageConstants.User_Password_Retry_Limit_Exceed, maxRetryCount, lockTime);
            Task.Factory.StartNew(async () =>
            {
                await _sysLogininforService.AddAsync(username, Constants.LOGIN_FAIL, retryLimitExceedMsg);
            });

            throw new ServiceException(retryLimitExceedMsg);
        }

        if (!Matches(user, password))
        {
            retryCount = retryCount + 1;

            var retryLimitCountMsg = string.Format(MessageConstants.User_Password_Retry_Limit_Count, retryCount);
            var notMatchMsg = MessageConstants.User_Passwrod_Not_Match;
            Task.Factory.StartNew(async () =>
            {
                await _sysLogininforService.AddAsync(username, Constants.LOGIN_FAIL, retryLimitCountMsg);
                await _sysLogininforService.AddAsync(username, Constants.LOGIN_FAIL, notMatchMsg);
            });

            _cache.SetString(GetCacheKey(username), retryCount.ToString(), lockTime);

            throw new ServiceException(notMatchMsg);
        }
        else
        {
            ClearLoginRecordCache(username);
        }
    }

    public bool Matches(SysUserDto user, string rawPassword)
    {
        return SecurityUtils.MatchesPassword(rawPassword, user.Password!);
    }

    public void ClearLoginRecordCache(string loginName)
    {
        _cache.Remove(GetCacheKey(loginName));
    }
}
