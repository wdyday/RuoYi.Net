using Microsoft.Extensions.Logging;
using RuoYi.Framework;

namespace RuoYi.System.Controllers
{
    /// <summary>
    /// 数据监控(暂无此功能)
    /// </summary>
    [Route("monitor/druid")]
    [ApiDescriptionSettings("Monitor")]
    public class DruidController : ControllerBase
    {
        private readonly ILogger<SysOperLogController> _logger;

        public DruidController(ILogger<SysOperLogController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 数据监控
        /// </summary>
        [HttpGet("")]
        [AppAuthorize("monitor:druid:list")]
        public AjaxResult GetDruidInfo()
        {
            return AjaxResult.Success();
        }
    }
}