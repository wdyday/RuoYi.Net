using Furion;
using Furion.DataEncryption;
using Furion.Logging;
using Microsoft.AspNetCore.Http;
using RuoYi.Data.Dtos;
using RuoYi.Data.Models;
using RuoYi.Framework.Exceptions;
using RuoYi.Framework.Redis;
using System.Security.Claims;

namespace RuoYi.Data.Utils
{
    public static class SecurityUtils
    {
        private static RedisCache _redisCache = App.GetService<RedisCache>();

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

        private static LoginUser GetCurrentUser()
        {
            // 解析对应的权限以及用户信息
            string uuid = App.User.FindFirstValue(Constants.LOGIN_USER_KEY) ?? "";
            string userKey = GetTokenKey(uuid);
            return _redisCache.Get<LoginUser>(userKey);
        }

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
