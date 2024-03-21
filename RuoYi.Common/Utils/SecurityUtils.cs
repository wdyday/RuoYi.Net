using Microsoft.AspNetCore.Http;
using RuoYi.Data;
using RuoYi.Data.Dtos;
using RuoYi.Data.Models;
using RuoYi.Framework;
using RuoYi.Framework.Cache;
using RuoYi.Framework.DataEncryption;
using RuoYi.Framework.Exceptions;
using RuoYi.Framework.JwtBearer;
using RuoYi.Framework.Logging;
using System.Security.Claims;

namespace RuoYi.Common.Utils
{
    public static class SecurityUtils
    {
        private static ICache _cache = App.GetService<ICache>();

        /// <summary>
        /// 登录用户ID
        /// </summary>
        public static long GetUserId()
        {
            var user = GetCurrentUser();
            return user?.UserId ?? 0;
        }

        /// <summary>
        /// 获取部门ID
        /// </summary>
        public static long? GetDeptId()
        {
            var user = GetCurrentUser();
            return user?.DeptId ?? null;
        }

        /**
         * 获取用户账户
         **/
        public static string? GetUsername()
        {
            var user = GetCurrentUser();
            return user?.UserName;
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        public static LoginUser GetLoginUser()
        {
            LoginUser user = GetCurrentUser();
            return user == null ? throw new ServiceException("获取用户信息异常", StatusCodes.Status401Unauthorized) : user;
        }

        public static LoginUser GetLoginUser(HttpRequest request)
        {
            // 获取请求携带的令牌
            string token = GetToken(request);
            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var claims = ParseToken(token);
                    // 解析对应的权限以及用户信息
                    string uuid = claims.Where(c => c.Type.Equals(RuoYi.Data.Constants.LOGIN_USER_KEY)).First().Value;
                    string userKey = GetTokenKey(uuid);
                    LoginUser user = _cache.Get<LoginUser>(userKey);
                    return user;
                }
                catch (Exception e)
                {
                    Log.Error("获取用户信息异常'{}'", e.Message);
                }
            }
            return null;
        }

        private static LoginUser GetCurrentUser()
        {
            return GetLoginUser(App.HttpContext.Request);
        }

        #region Token

        public static IEnumerable<Claim> ParseToken(string token)
        {
            var jwtSecurityToken = JWTEncryption.SecurityReadJwtToken(token);
            return jwtSecurityToken.Claims;
        }

        /// <summary>
        /// 获取请求token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetToken(HttpRequest request)
        {
            string token = request.Headers["Authorization"]!;
            if (!string.IsNullOrEmpty(token) && token.StartsWith(RuoYi.Data.Constants.TOKEN_PREFIX))
            {
                token = token.Replace(RuoYi.Data.Constants.TOKEN_PREFIX, "");
            }
            return token;
        }

        #endregion

        /// <summary>
        /// 生成BCryptPasswordEncoder密码
        /// </summary>
        /// <param name="password">原始密码</param>
        /// <returns>加密字符串</returns>
        public static string EncryptPassword(string password)
        {
            return MD5Encryption.Encrypt(password);
        }

        /// <summary>
        /// 判断密码是否相同
        /// </summary>
        /// <param name="rawPassword">原始密码</param>
        /// <param name="encodedPassword">加密后字符</param>
        /// <returns>结果</returns>
        public static bool MatchesPassword(string rawPassword, string encodedPassword)
        {
            return MD5Encryption.Compare(rawPassword, encodedPassword);
        }

        #region 是否为管理员
        /// <summary>
        /// 是否为管理员
        /// </summary>
        /// <param name="userId">用户ID</param>
        public static bool IsAdmin(long? userId)
        {
            return userId != null && 1L == userId;
        }

        public static bool IsAdmin()
        {
            var userId = GetUserId();
            return IsAdmin(userId);
        }

        public static bool IsAdmin(SysUserDto user)
        {
            return IsAdmin(user?.UserId);
        }
        #endregion

        /// <summary>
        /// 是否为管理员角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        public static bool IsAdminRole(long? roleId)
        {
            return roleId != null && 1L == roleId;
        }

        public static string GetTokenKey(string uuid)
        {
            return CacheConstants.LOGIN_TOKEN_KEY + uuid;
        }
    }
}
