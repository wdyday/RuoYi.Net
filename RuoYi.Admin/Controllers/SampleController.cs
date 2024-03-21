using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Logging;
using RuoYi.Data.Slave.Dtos;
using RuoYi.Framework.RateLimit;

namespace RuoYi.Admin
{
    /// <summary>
    /// 示例接口
    /// </summary>
    [ApiDescriptionSettings("Sample")]
    public class SampleController : ControllerBase
    {
        private readonly ILogger<SampleController> _logger;

        private readonly SystemService _systemService;
        private readonly RuoYi.System.Slave.Services.SysUserService _slaveSysUserService;

        public SampleController(ILogger<SampleController> logger, SystemService systemService, 
            System.Slave.Services.SysUserService slaveSysUserService)
        {
            _logger = logger;
            _systemService = systemService;
            _slaveSysUserService = slaveSysUserService;
        }

        /// <summary>
        /// 从库(slave) 用户查询
        /// 表 SysUser 的实体类上 添加特性 [Tenant(DataConstants.Slave)]
        /// </summary>
        [HttpGet("{id}")]
        public async Task<SlaveSysUserDto> Get(long? id)
        {
            return await _slaveSysUserService.GetAsync(id);
        }

        /// <summary>
        /// 限流
        /// 添加特性 [EnableRateLimiting(LimitType.Default)]
        /// </summary>
        [HttpGet("rateLimit")]
        [EnableRateLimiting(LimitType.Default)]
        public string RateLimit()
        {
            //return Guid.NewGuid().ToString();
            return "rateLimit";
        }

        /// <summary>
        /// IP限流
        /// 添加特性 [EnableRateLimiting(LimitType.IP)]
        /// </summary>
        [HttpGet("ipRateLimit")]
        [EnableRateLimiting(LimitType.IP)]
        public string IpRateLimit()
        {
            //return Guid.NewGuid().ToString();
            return "ipRateLimit";
        }
    }
}