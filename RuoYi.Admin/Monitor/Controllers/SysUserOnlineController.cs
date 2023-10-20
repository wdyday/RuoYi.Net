using Microsoft.Extensions.Logging;
using RuoYi.Common.Enums;
using RuoYi.Common.Utils;
using RuoYi.Data;
using RuoYi.Data.Models;
using RuoYi.Framework;
using RuoYi.Framework.Redis;
using RuoYi.Framework.Utils;
using RuoYi.System.Services;

namespace RuoYi.System.Controllers
{
    /// <summary>
    /// 在线用户监控
    /// </summary>
    [Route("monitor/online")]
    [ApiDescriptionSettings("Monitor")]
    public class SysUserOnlineController : ControllerBase
    {
        private readonly ILogger<SysOperLogController> _logger;
        private readonly RedisCache _redisCache;
        private readonly SysUserOnlineService _sysUserOnlineService;

        public SysUserOnlineController(ILogger<SysOperLogController> logger,
            RedisCache redisCache,
            SysUserOnlineService sysUserOnlineService)
        {
            _logger = logger;
            _redisCache = redisCache;
            _sysUserOnlineService = sysUserOnlineService;
        }

        /// <summary>
        /// 查询操作日志记录列表
        /// </summary>
        [HttpGet("list")]
        [AppAuthorize("monitor:online:list")]
        public async Task<SqlSugarPagedList<SysUserOnline>> GetSysOperLogList([FromQuery] string ipaddr, [FromQuery] string userName)
        {
            var keys = _redisCache.GetStringKeys(CacheConstants.LOGIN_TOKEN_KEY + "*", 10000); // 取1万, 同时在线用户应该不会超过1万

            List<SysUserOnline> userOnlineList = new List<SysUserOnline>();
            foreach (var key in keys)
            {
                LoginUser user = await _redisCache.GetAsync<LoginUser>(key);
                if (StringUtils.IsNotEmpty(ipaddr) && StringUtils.IsNotEmpty(userName))
                {
                    userOnlineList.Add(_sysUserOnlineService.GetOnlineByInfo(ipaddr, userName, user));
                }
                else if (StringUtils.IsNotEmpty(ipaddr))
                {
                    userOnlineList.Add(_sysUserOnlineService.GetOnlineByIpaddr(ipaddr, user));
                }
                else if (StringUtils.IsNotEmpty(userName) && user.User != null)
                {
                    userOnlineList.Add(_sysUserOnlineService.GetOnlineByUserName(userName, user));
                }
                else
                {
                    userOnlineList.Add(_sysUserOnlineService.LoginUserToUserOnline(user));
                }
            }
            userOnlineList = userOnlineList.Where(u => u != null).ToList();

            return new SqlSugarPagedList<SysUserOnline>
            {
                Rows = userOnlineList,
                Total = userOnlineList.Count
            };
        }

        /// <summary>
        /// 强退用户
        /// </summary>
        [HttpDelete("{tokenId}")]
        [AppAuthorize("monitor:online:forceLogout")]
        [RuoYi.System.Log(Title = "在线用户", BusinessType = BusinessType.FORCE)]
        public AjaxResult ForceLogout(string tokenId)
        {
            _redisCache.Remove(CacheConstants.LOGIN_TOKEN_KEY + tokenId);
            return AjaxResult.Success();
        }
    }
}