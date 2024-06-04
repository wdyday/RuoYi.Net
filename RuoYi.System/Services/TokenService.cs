using Microsoft.IdentityModel.JsonWebTokens;
using RuoYi.Common.Utils;
using RuoYi.Data.Models;
using RuoYi.Framework.Cache;
using RuoYi.Framework.JwtBearer;
using UAParser.Interfaces;

namespace RuoYi.System.Services;

public class TokenService : ITransient
{
    protected static long MILLIS_SECOND = 1000;

    protected static long MILLIS_MINUTE = 60 * MILLIS_SECOND;

    private static long MILLIS_MINUTE_TEN = 20 * 60 * 1000L; // 20分钟

    private static long DEFAULT_EXPIREDTIME = 30;   // 默认过期 30分钟

    // JWT 过期分钟数: 一周. token有效期在redis中控制, JWT中存储的信息过期时间较长, 方便系统读取
    private static long JWT_EXPIREDTIME = 60 * 24 * 7;

    private readonly IUserAgentParser _userAgentParser;
    private readonly ICache _cache;

    public TokenService(IUserAgentParser userAgentParser, ICache cache)
    {
        _userAgentParser = userAgentParser;
        _cache = cache;
    }

    /// <summary>
    /// 获取用户身份信息
    /// </summary>
    public LoginUser GetLoginUser(HttpRequest request)
    {
        return SecurityUtils.GetLoginUser(request);
    }

    public void DelLoginUser(string token)
    {
        if (!string.IsNullOrEmpty(token))
        {
            string userKey = GetTokenKey(token);
            _cache.Remove(userKey);
        }
    }

    /// <summary>
    /// 设置用户身份信息
    /// </summary>
    public void SetLoginUser(LoginUser loginUser)
    {
        if (loginUser != null && !string.IsNullOrEmpty(loginUser.Token))
        {
            RefreshToken(loginUser);
        }
    }

    /// <summary>
    /// 创建令牌
    /// </summary>
    /// <param name="loginUser">用户信息</param>
    /// <returns></returns>
    public async Task<string> CreateToken(LoginUser loginUser)
    {
        var token = Guid.NewGuid().ToString();
        loginUser.Token = token;
        await SetUserAgent(loginUser);
        RefreshToken(loginUser);

        // 生成 token
        var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>()
        {
            { Constants.LOGIN_USER_KEY, token },
            { DataConstants.USER_ID, loginUser.UserId },
            { DataConstants.USER_NAME, loginUser.UserName },
            { DataConstants.USER_DEPT_ID, loginUser.DeptId },
            { JwtRegisteredClaimNames.Exp, DateTimeOffset.UtcNow.AddMinutes(JWT_EXPIREDTIME).ToUnixTimeSeconds() }
        });
        return accessToken;
    }

    /// <summary>
    /// 验证令牌有效期，相差不足20分钟，自动刷新缓存
    /// </summary>
    public void VerifyToken(LoginUser loginUser)
    {
        long expireTime = loginUser.ExpireTime;
        long currentTime = DateTime.Now.ToUnixTimeMilliseconds();
        if (expireTime - currentTime <= MILLIS_MINUTE_TEN)
        {
            RefreshToken(loginUser);
        }
    }

    /// <summary>
    /// 刷新令牌有效期
    /// </summary>
    /// <param name="loginUser">用户信息</param>
    public void RefreshToken(LoginUser loginUser)
    {
        var jwtSettings = App.GetConfig<JWTSettingsOptions>("JWTSettings");
        long expireTime = jwtSettings.ExpiredTime ?? DEFAULT_EXPIREDTIME;

        loginUser.LoginTime = DateTime.Now.ToUnixTimeMilliseconds();
        loginUser.ExpireTime = loginUser.LoginTime + expireTime * MILLIS_MINUTE;
        // 根据uuid将loginUser缓存
        string userKey = GetTokenKey(loginUser.Token);
        _cache.Set<LoginUser>(userKey, loginUser, expireTime);
    }

    /// <summary>
    /// 设置用户代理信息
    /// </summary>
    /// <param name="loginUser">用户信息</param>
    public async Task SetUserAgent(LoginUser loginUser)
    {
        var clientInfo = this._userAgentParser.ClientInfo;

        string ip = App.HttpContext.GetRemoteIpAddressToIPv4();

        loginUser.IpAddr = ip;
        loginUser.LoginLocation = await AddressUtils.GetRealAddressByIPAsync(ip);
        loginUser.Browser = clientInfo.Browser.ToString();
        loginUser.OS = clientInfo.OS.Family.ToString();
    }

    private string GetTokenKey(string uuid)
    {
        return SecurityUtils.GetTokenKey(uuid);
    }
}
