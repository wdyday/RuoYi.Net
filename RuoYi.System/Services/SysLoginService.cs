using Lazy.Captcha.Core;
using RuoYi.Common.Constants;
using RuoYi.Common.Enums;
using RuoYi.Data.Models;
using RuoYi.Framework.Cache;
using RuoYi.Framework.Exceptions;

namespace RuoYi.System.Services;

public class SysLoginService : ITransient
{
    private readonly ILogger<SysLoginService> _logger;
    private readonly ICaptcha _captcha;
    private readonly ICache _cache;
    private readonly TokenService _tokenService;
    private readonly SysUserService _sysUserService;
    private readonly SysConfigService _sysConfigService;
    private readonly SysLogininforService _sysLogininforService;
    private readonly SysPasswordService _sysPasswordService;
    private readonly SysPermissionService _sysPermissionService;

    public SysLoginService(ILogger<SysLoginService> logger, ICaptcha captcha,
        ICache cache, TokenService tokenService,
        SysUserService sysUserService, SysConfigService sysConfigService,
        SysLogininforService sysLogininforService, SysPasswordService sysPasswordService,
        SysPermissionService sysPermissionService)
    {
        _logger = logger;
        _captcha = captcha;
        _cache = cache;
        _tokenService = tokenService;
        _sysUserService = sysUserService;
        _sysConfigService = sysConfigService;
        _sysLogininforService = sysLogininforService;
        _sysPasswordService = sysPasswordService;
        _sysPermissionService = sysPermissionService;
    }

    /// <summary>
    /// 登录验证
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="password">密码</param>
    /// <param name="code">验证码</param>
    /// <param name="uuid">唯一标识</param>
    /// <returns>结果</returns>
    public async Task<string> LoginAsync(string username, string password, string code, string uuid)
    {
        // 验证码校验
        ValidateCaptcha(username, code, uuid);
        // 登录前置校验
        LoginPreCheck(username, password);
        // 用户验证
        var userDto = await _sysUserService.GetDtoByUsernameAsync(username);
        CheckLoginUser(username, password, userDto);

        // 记录登录成功
        await _sysLogininforService.AddAsync(username, Constants.LOGIN_SUCCESS, MessageConstants.User_Login_Success);

        var loginUser = CreateLoginUser(userDto);
        await RecordLoginInfoAsync(userDto.UserId ?? 0);

        // 生成token
        return await _tokenService.CreateToken(loginUser);
    }

    private void CheckLoginUser(string username, string password, SysUserDto user)
    {
        if (user == null)
        {
            _logger.LogInformation($"登录用户：{username} 不存在.");
            throw new ServiceException(MessageConstants.User_Passwrod_Not_Match);
        }
        else if (UserStatus.DELETED.GetValue().Equals(user.DelFlag))
        {
            _logger.LogInformation($"登录用户：{username} 已被删除.");
            throw new ServiceException(MessageConstants.User_Deleted);
        }
        else if (UserStatus.DISABLE.GetValue().Equals(user.Status))
        {
            _logger.LogInformation($"登录用户：{username} 已被停用.");
            throw new ServiceException(MessageConstants.User_Blocked);
        }

        // 密码验证
        _sysPasswordService.Validate(username, password, user);
    }

    /// <summary>
    /// 校验验证码
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="code">验证码</param>
    /// <param name="uuid">唯一标识</param>
    private void ValidateCaptcha(string username, string code, string uuid)
    {
        bool captchaEnabled = _sysConfigService.IsCaptchaEnabled();
        if (captchaEnabled)
        {
            // 无论验证是否通过, 都删除缓存的验证码
            var isValidCaptcha = _captcha.Validate(uuid, code, true, true);
            if (!isValidCaptcha)
            {
                Task.Factory.StartNew(async () =>
                {
                    await _sysLogininforService.AddAsync(username, Constants.LOGIN_FAIL, MessageConstants.Captcha_Invalid);
                });
                throw new ServiceException(MessageConstants.Captcha_Invalid);
            }
        }
    }

    /// <summary>
    /// 登录前置校验
    /// </summary>
    /// <param name="username">用户名</param>
    /// <param name="password">用户密码</param>
    private void LoginPreCheck(string username, string password)
    {
        // 用户名或密码为空 错误
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Task.Factory.StartNew(async () =>
            {
                await _sysLogininforService.AddAsync(username, Constants.LOGIN_FAIL, MessageConstants.Required);
            });
            throw new ServiceException(MessageConstants.Required);
        }
        // 密码如果不在指定范围内 错误
        if (password.Length < UserConstants.PASSWORD_MIN_LENGTH || password.Length > UserConstants.PASSWORD_MAX_LENGTH)
        {
            Task.Factory.StartNew(async () =>
            {
                await _sysLogininforService.AddAsync(username, Constants.LOGIN_FAIL, MessageConstants.User_Passwrod_Not_Match);
            });
            throw new ServiceException(MessageConstants.User_Passwrod_Not_Match);
        }
        // 用户名不在指定范围内 错误
        if (username.Length < UserConstants.USERNAME_MIN_LENGTH || username.Length > UserConstants.USERNAME_MAX_LENGTH)
        {
            Task.Factory.StartNew(async () =>
            {
                await _sysLogininforService.AddAsync(username, Constants.LOGIN_FAIL, MessageConstants.User_Passwrod_Not_Match);
            });
            throw new ServiceException(MessageConstants.User_Passwrod_Not_Match);
        }
        // IP黑名单校验
        string? blackStr = _cache.GetString("sys.login.blackIPList");
        if (IpUtils.IsMatchedIp(blackStr, App.HttpContext.GetRemoteIpAddressToIPv4()))
        {
            Task.Factory.StartNew(async () =>
            {
                await _sysLogininforService.AddAsync(username, Constants.LOGIN_FAIL, MessageConstants.Login_Blocked);
            });
            throw new ServiceException(MessageConstants.Login_Blocked);
        }
    }

    private LoginUser CreateLoginUser(SysUserDto user)
    {
        var permissions = _sysPermissionService.GetMenuPermission(user);
        return new LoginUser
        {
            UserId = user.UserId ?? 0,
            DeptId = user.DeptId ?? 0,
            UserName = user.UserName ?? "",
            Password = user.Password ?? "",
            User = user,
            Permissions = permissions
        };
    }

    /// <summary>
    /// 记录登录信息
    /// </summary>
    public async Task RecordLoginInfoAsync(long userId)
    {
        SysUserDto sysUser = new SysUserDto();
        sysUser.UserId = userId;
        sysUser.LoginIp = IpUtils.GetIpAddr();
        sysUser.LoginDate = DateTime.Now;
        await _sysUserService.UpdateUserLoginInfoAsync(sysUser);
    }
}
