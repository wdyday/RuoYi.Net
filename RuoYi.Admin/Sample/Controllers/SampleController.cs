using Microsoft.Extensions.Logging;
using RuoYi.Data.Slave.Dtos;

namespace RuoYi.Admin.Sample
{
    /// <summary>
    /// 示例接口
    /// </summary>
    [ApiDescriptionSettings("Sample")]
    public class SampleController : IDynamicApiController
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
        public async Task<SysUserDto> Get(long? id)
        {
            return await _slaveSysUserService.GetAsync(id);
        }
    }
}