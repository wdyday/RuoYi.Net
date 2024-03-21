using RuoYi.Data.Models;

namespace RuoYi.System.Services;

/// <summary>
/// 在线用户 服务层处理
/// </summary>
public class SysUserOnlineService : ITransient
{
    /// <summary>
    /// 通过登录地址查询信息
    /// </summary>
    /// <param name="ipaddr">登录地址</param>
    /// <param name="user">用户信息</param>
    /// <returns>在线用户信息</returns>
    public SysUserOnline? GetOnlineByIpaddr(string ipaddr, LoginUser user)
    {
        if (StringUtils.Equals(ipaddr, user.IpAddr))
        {
            return LoginUserToUserOnline(user);
        }
        return null;
    }

    /// <summary>
    /// 通过用户名称查询信息
    /// </summary>
    /// <param name="userName">用户名称</param>
    /// <param name="user">用户信息</param>
    /// <returns>在线用户信息</returns>
    public SysUserOnline? GetOnlineByUserName(string userName, LoginUser user)
    {
        if (StringUtils.Equals(userName, user.UserName))
        {
            return LoginUserToUserOnline(user);
        }
        return null;
    }

    /// <summary>
    /// 通过登录地址/用户名称查询信息
    /// </summary>
    /// <param name="ipaddr">登录地址</param>
    /// <param name="userName">用户名称</param>
    /// <param name="user">用户信息</param>
    /// <returns>在线用户信息</returns>
    public SysUserOnline? GetOnlineByInfo(string ipaddr, string userName, LoginUser user)
    {
        if (StringUtils.Equals(ipaddr, user.IpAddr) && StringUtils.Equals(userName, user.UserName))
        {
            return LoginUserToUserOnline(user);
        }
        return null;
    }

    /// <summary>
    /// 设置在线用户信息
    /// </summary>
    /// <param name="user">用户信息</param>
    /// <returns>在线用户</returns>
    public SysUserOnline? LoginUserToUserOnline(LoginUser user)
    {
        if (user == null || user.User == null)
        {
            return null;
        }
        return new SysUserOnline
        {
            TokenId = user.Token,
            UserName = user.UserName,
            Ipaddr = user.IpAddr,
            LoginLocation = user.LoginLocation,
            Browser = user.Browser,
            Os = user.OS,
            LoginTime = user.LoginTime,
            DeptName = user.User.Dept?.DeptName
        };
    }
}
