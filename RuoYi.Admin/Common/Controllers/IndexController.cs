using Microsoft.Extensions.Logging;
using StackExchange.Profiling;

namespace RuoYi.Admin
{
    /// <summary>
    /// 系统服务接口
    /// </summary>
    [ApiDescriptionSettings("Common")]
    public class IndexController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<IndexController> _logger;

        private readonly SystemService _systemService;
        public IndexController(IHttpContextAccessor httpContextAccessor, ILogger<IndexController> logger, SystemService systemService)
        {
            _httpContextAccessor = httpContextAccessor;
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

        /// <summary>
        /// 获取 MiniProfiler 脚本引用 script
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetMiniProfilerIncludeScript")]
        public string GetMiniProfilerIncludeScript()
        {
            var script = MiniProfiler.Current.RenderIncludes(_httpContextAccessor.HttpContext);
            return script.Value;
        }
    }
}