using RuoYi.Common.Enums;
using RuoYi.Common.Utils;
using RuoYi.Quartz.Services;
using RuoYi.System;

namespace RuoYi.Quartz.Controllers
{
    [Route("monitor/jobLog")]
    [ApiDescriptionSettings("Monitor")]
    public class SysJobLogController : ControllerBase
    {
        private readonly SysJobLogService _sysJobLogService;

        public SysJobLogController(SysJobLogService sysJobLogService)
        {
            _sysJobLogService = sysJobLogService;
        }

        /// <summary>
        /// 查询定时任务调度日志列表
        /// </summary>
        [HttpGet("list")]
        [AppAuthorize("monitor:job:list")]
        public async Task<SqlSugarPagedList<SysJobLogDto>> GetSysJobLogPagedList([FromQuery] SysJobLogDto dto)
        {
            return await _sysJobLogService.GetDtoPagedListAsync(dto);
        }

        /// <summary>
        /// 根据调度编号获取详细信息
        /// </summary>
        [HttpGet("")]
        [HttpGet("{jobLogId}")]
        [AppAuthorize("monitor:job:query")]
        public async Task<AjaxResult> Get(long jobLogId)
        {
            var data = await _sysJobLogService.GetDtoAsync(jobLogId);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 删除 定时任务调度日志
        /// </summary>
        [HttpDelete("{jobLogIds}")]
        [AppAuthorize("monitor:job:remove")]
        [RuoYi.System.Log(Title = "定时任务调度日志", BusinessType = BusinessType.DELETE)]
        public async Task<AjaxResult> Remove([ModelBinder] long[] jobLogIds)
        {
            var data = await _sysJobLogService.DeleteAsync(jobLogIds);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 清空 定时任务调度日志
        /// </summary>
        [HttpDelete("clean")]
        [AppAuthorize("monitor:job:remove")]
        [RuoYi.System.Log(Title = "定时任务调度日志", BusinessType = BusinessType.DELETE)]
        public AjaxResult Clean()
        {
            _sysJobLogService.Clean();
            return AjaxResult.Success();
        }

        /// <summary>
        /// 导出 定时任务调度日志列表
        /// </summary>
        [HttpPost("export")]
        [AppAuthorize("monitor:job:export")]
        [Log(Title = "任务调度日志", BusinessType = BusinessType.EXPORT)]
        public async Task Export([FromForm] SysJobLogDto dto)
        {
            var dtos = await _sysJobLogService.GetDtoListAsync(dto);
            await ExcelUtils.ExportAsync(App.HttpContext.Response, dtos, sheetName: "调度日志");
        }
    }
}
