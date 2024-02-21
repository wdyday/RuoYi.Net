﻿using RuoYi.Framework.Authorization;

namespace Microsoft.AspNetCore.Authorization;

/// <summary>
/// 策略授权特性
/// </summary>
[SuppressSniffer, AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AppAuthorizeAttribute : AuthorizeAttribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="policies">多个策略</param>
    public AppAuthorizeAttribute(params string[] policies)
    {
        if (policies != null && policies.Length > 0) Policies = policies;
    }

    /// <summary>
    /// 策略
    /// </summary>
    public string[] Policies
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Policy)) return Array.Empty<string>();

            return Policy[Penetrates.AppAuthorizePrefix.Length..].Split(',', StringSplitOptions.RemoveEmptyEntries);
        }
        internal set => Policy = $"{Penetrates.AppAuthorizePrefix}{string.Join(',', value)}";
    }
}