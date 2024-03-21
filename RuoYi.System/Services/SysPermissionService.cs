using RuoYi.Common.Utils;

namespace RuoYi.System.Services;

/// <summary>
/// 用户权限处理
/// </summary>
public class SysPermissionService : ITransient
{
    private readonly SysRoleService _sysRoleService;
    private readonly SysMenuService _sysMenuService;

    public SysPermissionService(SysRoleService sysRoleService, SysMenuService sysMenuService)
    {
        _sysRoleService = sysRoleService;
        _sysMenuService = sysMenuService;
    }

    /// <summary>
    /// 获取角色数据权限
    /// </summary>
    /// <param name="user">用户信息</param>
    /// <returns>角色权限信息</returns>
    public async Task<List<string>> GetRolePermissionAsync(SysUserDto user)
    {
        var roles = new List<string>();
        // 管理员拥有所有权限
        if (SecurityUtils.IsAdmin(user.UserId))
        {
            roles.Add("admin");
        }
        else
        {
            roles.AddRange(await _sysRoleService.GetRolePermissionByUserId(user.UserId!.Value));
        }
        return roles;
    }

    public List<string> GetMenuPermission(SysUserDto user)
    {
        List<string> perms = new List<string>();
        // 管理员拥有所有权限
        if (SecurityUtils.IsAdmin(user.UserId))
        {
            perms.Add("*:*:*");
        }
        else
        {
            List<SysRoleDto> roles = user.Roles!;
            if (roles.IsNotEmpty())
            {
                // 多角色设置permissions属性，以便数据权限匹配权限
                foreach (SysRoleDto role in roles)
                {
                    List<string> rolePerms = _sysMenuService.SelectMenuPermsByRoleId(role.RoleId);
                    role.Permissions = rolePerms;
                    perms.AddRange(rolePerms);
                }
            }
            else
            {
                perms.AddRange(_sysMenuService.SelectMenuPermsByUserId(user.UserId!.Value));
            }
        }
        return perms;
    }
}
