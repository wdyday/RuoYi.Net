using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using RuoYi.Common.Interceptors;
using RuoYi.Common.Utils;
using RuoYi.Data.Models;
using RuoYi.Framework.Utils;
using RuoYi.System.Services;

namespace RuoYi.Admin.Authorization
{
    public class AuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        /** 所有权限标识 */
        private static string ALL_PERMISSION = "*:*:*";

        /** 管理员角色权限标识 */
        private static string SUPER_ADMIN = "admin";

        //private static string ROLE_DELIMETER = ",";

        //private static string PERMISSION_DELIMETER = ",";


        public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            // 判断是否授权
            var tokenService = App.GetService<TokenService>();
            LoginUser loginUser = tokenService.GetLoginUser(context.Request);
            if (loginUser != null)
            {
                var isAuthenticated = CheckAuthorize(context);
                if (isAuthenticated)
                {
                    tokenService.VerifyToken(loginUser);
                    await next(context!);
                }
                else
                {
                    // Unauthorized
                    var result = AjaxResult.Error(StatusCodes.Status403Forbidden, msg: "403 Forbidden");
                    await context.Response.WriteAsJsonAsync(result, App.GetOptions<JsonOptions>()?.JsonSerializerOptions);
                }
            }
            else
            {
                context?.SignoutToSwagger();    // 退出Swagger登录

                // Unauthorized
                var result = AjaxResult.Error(StatusCodes.Status401Unauthorized, msg: "401 Unauthorized");
                await context.Response.WriteAsJsonAsync(result, App.GetOptions<JsonOptions>()?.JsonSerializerOptions);
            }
        }

        /// <summary>
        /// 检查 Contoller的 AppAuthorizeAttribute 授权
        /// </summary>
        private static bool CheckAuthorize(HttpContext httpContext)
        {
            // 获Contoller取权限特性
            var appAuthorizeAttribute = httpContext.GetMetadata<AppAuthorizeAttribute>();
            var appRoleAuthorizeAttribute = httpContext.GetMetadata<AppRoleAuthorizeAttribute>();
            if (appAuthorizeAttribute == null && appRoleAuthorizeAttribute == null) return true;

            // 角色验证
            if (appRoleAuthorizeAttribute != null)
            {
                if (!HasAnyRoles(appRoleAuthorizeAttribute.AppRoles))
                {
                    return false;
                }
            }

            // 权限验证
            if(appAuthorizeAttribute != null)
            {
                if (!HasAnyPermi(appAuthorizeAttribute.Policies))
                {
                    return false;
                }
            }

            return true;
        }

        #region Permi
        /// <summary>
        /// 判断是否包含权限
        /// </summary>
        private static bool HasPermi(string permission)
        {
            if (string.IsNullOrEmpty(permission)) return false;

            var tokenService = App.GetService<TokenService>();
            var loginUser = tokenService.GetLoginUser(App.HttpContext.Request);
            if (loginUser == null || loginUser.Permissions.IsEmpty())
            {
                return false;
            }

            PermissionContextHolder.SetContext(permission);

            return HasPermissions(loginUser.Permissions, permission);
        }

        /// <summary>
        /// 判断是否包含权限
        /// </summary>
        private static bool HasAnyPermi(string[] permissions)
        {
            if (permissions.IsEmpty()) return false;

            var tokenService = App.GetService<TokenService>();
            var loginUser = tokenService.GetLoginUser(App.HttpContext.Request);
            if (loginUser == null || loginUser.Permissions.IsEmpty())
            {
                return false;
            }
            foreach (var permission in permissions)
            {
                if (HasPermissions(loginUser.Permissions, permission))
                {
                    PermissionContextHolder.SetContext(permission);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 判断是否包含权限
        /// </summary>
        private static bool HasPermissions(List<string> permissions, string permission)
        {
            return permissions.Contains(ALL_PERMISSION) || permissions.Contains(permission.Trim());
        }
        #endregion

        #region Role
        /// <summary>
        /// 判断用户是否拥有某个角色
        /// </summary>
        /// <param name="role">角色字符串</param>
        /// <returns>用户是否具备某角色</returns>
        public static bool HasRole(string role)
        {
            if (StringUtils.IsEmpty(role))
            {
                return false;
            }
            LoginUser loginUser = SecurityUtils.GetLoginUser();
            if (loginUser == null || loginUser.User.Roles == null || loginUser.User.Roles.Count == 0)
            {
                return false;
            }
            foreach (var sysRole in loginUser.User.Roles)
            {
                string roleKey = sysRole.RoleKey;
                if (SUPER_ADMIN == roleKey || roleKey == StringUtils.TrimToEmpty(role))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 验证用户是否具有以下任意一个角色
        /// </summary>
        /// <param name="roles">以 ROLE_NAMES_DELIMETER 为分隔符的角色列表</param>
        /// <returns>用户是否具有以下任意一个角色</returns>
        public static bool HasAnyRoles(string[] roles)
        {
            if (roles == null && roles.Length == 0)
            {
                return false;
            }

            LoginUser loginUser = SecurityUtils.GetLoginUser();
            if (loginUser == null || loginUser.User.Roles == null || loginUser.User.Roles.Count == 0)
            {
                return false;
            }
            foreach (string role in roles)
            {
                if (HasRole(role))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
