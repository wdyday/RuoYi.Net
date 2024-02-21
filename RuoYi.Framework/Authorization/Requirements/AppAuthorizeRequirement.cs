using Microsoft.AspNetCore.Authorization;

namespace RuoYi.Framework.Authorization;

/// <summary>
/// 策略对应的需求
/// </summary>
[SuppressSniffer]
public sealed class AppAuthorizeRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="policies"></param>
    public AppAuthorizeRequirement(params string[] policies)
    {
        Policies = policies;
    }

    /// <summary>
    /// 策略
    /// </summary>
    public string[] Policies { get; private set; }
}