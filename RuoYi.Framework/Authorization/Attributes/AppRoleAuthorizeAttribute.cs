using RuoYi.Framework.Authorization;

namespace Microsoft.AspNetCore.Authorization;

/// <summary>
/// 角色授权特性
/// </summary>
[SuppressSniffer, AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
public sealed class AppRoleAuthorizeAttribute : AuthorizeAttribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="roles">多个角色</param>
    public AppRoleAuthorizeAttribute(params string[] roles)
    {
        if (roles != null && roles.Length > 0) AppRoles = roles;
    }

    /// <summary>
    /// 角色
    /// </summary>
    public string[] AppRoles
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Roles)) return Array.Empty<string>();

            return Roles[Penetrates.AppAuthorizePrefix.Length..].Split(',', StringSplitOptions.RemoveEmptyEntries);
        }
        internal set => Roles = $"{Penetrates.AppAuthorizePrefix}{string.Join(',', value)}";
    }
}