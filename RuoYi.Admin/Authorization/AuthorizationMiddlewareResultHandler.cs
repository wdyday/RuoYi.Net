using Microsoft.AspNetCore.Authorization.Policy;
using RuoYi.Common.Interceptors;
using RuoYi.Data.Models;
using RuoYi.System.Services;

namespace RuoYi.Admin.Authorization
{
    public class AuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        /** 所有权限标识 */
        private static string ALL_PERMISSION = "*:*:*";

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
            if (appAuthorizeAttribute == null) return true;


            // 角色验证
            //if (!string.IsNullOrEmpty(appAuthorizeAttribute.Roles))
            //{
            //    // TODO
            //}

            // 权限验证
            if (!HasAnyPermi(appAuthorizeAttribute.Policies))
            {
                return false;
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
    }
}
