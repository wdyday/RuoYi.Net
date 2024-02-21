using Microsoft.Extensions.Logging;

namespace RuoYi.Admin
{
    /// <summary>
    /// 系统服务接口
    /// </summary>
    [ApiDescriptionSettings("Common")]
    public class IndexController : ControllerBase
    {
        private readonly ILogger<IndexController> _logger;

        private readonly SystemService _systemService;
        public IndexController(ILogger<IndexController> logger, SystemService systemService)
        {
            _logger = logger;
            _systemService = systemService;
        }

        /// <summary>
        /// 获取系统描述
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetDescription")]
        public string GetDescription()
        {
            _logger.LogInformation("获取系统描述");
            return _systemService.GetDescription();
        }
    }
}