using RuoYi.Common.Enums;
using RuoYi.Common.Utils;
using RuoYi.Data.Dtos;
using RuoYi.System.Services;
using SqlSugar;

namespace RuoYi.System.Controllers
{
    /// <summary>
    /// 操作日志记录
    /// </summary>
    [Route("monitor/operlog")]
    [ApiDescriptionSettings("Monitor")]
    public class SysOperLogController : ControllerBase
    {
        private readonly ILogger<SysOperLogController> _logger;
        private readonly SysOperLogService _sysOperLogService;

        public SysOperLogController(ILogger<SysOperLogController> logger,
            SysOperLogService sysOperLogService)
        {
            _logger = logger;
            _sysOperLogService = sysOperLogService;
        }

        /// <summary>
        /// 查询操作日志记录列表
        /// </summary>
        [HttpGet("list")]
        [AppAuthorize("system:log:list")]
        public async Task<SqlSugarPagedList<SysOperLogDto>> GetSysOperLogList([FromQuery] SysOperLogDto dto)
        {
            return await _sysOperLogService.GetDtoPagedListAsync(dto);
        }

        /// <summary>
        /// 删除 操作日志记录
        /// </summary>
        [HttpDelete("{ids}")]
        [AppAuthorize("system:log:remove")]
        [RuoYi.System.Log(Title = "操作日志", BusinessType = BusinessType.DELETE)]
        public async Task<AjaxResult> Remove(string ids)
        {
            var idList = ids.SplitToList<long>();
            var data = await _sysOperLogService.DeleteAsync(idList);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 删除 操作日志记录
        /// </summary>
        [HttpDelete("clean")]
        [AppAuthorize("system:log:remove")]
        [RuoYi.System.Log(Title = "操作日志", BusinessType = BusinessType.CLEAN)]
        public AjaxResult Clean()
        {
            _sysOperLogService.Clean();
            return AjaxResult.Success();
        }

        /// <summary>
        /// 导出 操作日志记录
        /// </summary>
        [HttpPost("export")]
        [AppAuthorize("system:log:export")]
        [RuoYi.System.Log(Title = "操作日志", BusinessType = BusinessType.EXPORT)]
        public async Task Export(SysOperLogDto dto)
        {
            var list = await _sysOperLogService.GetDtoListAsync(dto);
            await ExcelUtils.ExportAsync(App.HttpContext.Response, list);
        }
    }
}