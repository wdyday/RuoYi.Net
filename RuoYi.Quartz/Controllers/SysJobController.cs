using RuoYi.Common.Enums;
using RuoYi.Common.Utils;
using RuoYi.Framework;
using RuoYi.Quartz.Dtos;
using RuoYi.Quartz.Services;
using RuoYi.System;

namespace RuoYi.Quartz.Controllers
{
    /// <summary>
    /// 调度任务信息操作处理
    /// </summary>
    [Route("monitor/job")]
    [ApiDescriptionSettings("Monitor")]
    public class SysJobController : ControllerBase
    {
        private readonly ILogger<SysJobController> _logger;
        private readonly SysJobService _sysJobService;

        public SysJobController(ILogger<SysJobController> logger,
            SysJobService sysJobService)
        {
            _logger = logger;
            _sysJobService = sysJobService;
        }

        /// <summary>
        /// 查询定时任务调度列表
        /// </summary>
        [HttpGet("list")]
        [AppAuthorize("monitor:job:list")]
        public async Task<SqlSugarPagedList<SysJobDto>> GetSysJobPagedList([FromQuery] SysJobDto dto)
        {
            return await _sysJobService.GetDtoPagedListAsync(dto);
        }

        /// <summary>
        /// 获取 定时任务调度 详细信息
        /// </summary>
        [HttpGet("")]
        [HttpGet("{jobId}")]
        [AppAuthorize("monitor:job:query")]
        public async Task<AjaxResult> Get(long jobId)
        {
            var data = await _sysJobService.GetDtoAsync(jobId);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 新增 定时任务调度
        /// </summary>
        [HttpPost("")]
        [AppAuthorize("monitor:job:add")]
        [TypeFilter(typeof(RuoYi.Framework.DataValidation.DataValidationFilter))]
        [RuoYi.System.Log(Title = "定时任务", BusinessType = BusinessType.INSERT)]
        public async Task<AjaxResult> Add([FromBody] SysJobDto job)
        {
            string msg = _sysJobService.CheckJob(job);
            if (!string.IsNullOrEmpty(msg))
            {
                return AjaxResult.Error($"新增任务{msg}");
            }

            var success = await _sysJobService.InsertJobAsync(job);
            return success ? AjaxResult.Success() : AjaxResult.Error();
        }

        /// <summary>
        /// 修改 定时任务调度
        /// </summary>
        [HttpPut("")]
        [AppAuthorize("monitor:job:edit")]
        [TypeFilter(typeof(RuoYi.Framework.DataValidation.DataValidationFilter))]
        [RuoYi.System.Log(Title = "定时任务", BusinessType = BusinessType.UPDATE)]
        public async Task<AjaxResult> Edit([FromBody] SysJobDto job)
        {
            string msg = _sysJobService.CheckJob(job);
            if (!string.IsNullOrEmpty(msg))
            {
                return AjaxResult.Error($"新增任务{msg}");
            }

            var success = await _sysJobService.UpdateJobAsync(job);
            return success ? AjaxResult.Success() : AjaxResult.Error();
        }

        /// <summary>
        /// 修改 定时任务调度
        /// </summary>
        [HttpPut("changeStatus")]
        [AppAuthorize("monitor:job:edit")]
        [RuoYi.System.Log(Title = "定时任务", BusinessType = BusinessType.UPDATE)]
        public async Task<AjaxResult> ChangeStatus([FromBody] SysJobDto dto)
        {
            var success = await _sysJobService.ChangeStatusAsync(dto);
            return success ? AjaxResult.Success() : AjaxResult.Error();
        }

        /// <summary>
        /// 定时任务立即执行一次
        /// </summary>
        [HttpPut("run")]
        [AppAuthorize("monitor:job:changeStatus")]
        [RuoYi.System.Log(Title = "定时任务", BusinessType = BusinessType.UPDATE)]
        public async Task<AjaxResult> Run([FromBody] SysJobDto dto)
        {
            bool result = await _sysJobService.Run(dto);
            return result ? AjaxResult.Success() : AjaxResult.Error("任务不存在或已过期！");
        }

        /// <summary>
        /// 删除 定时任务调度
        /// </summary>
        [HttpDelete("{jobIds}")]
        [AppAuthorize("monitor:job:remove")]
        [RuoYi.System.Log(Title = "定时任务", BusinessType = BusinessType.DELETE)]
        public async Task<AjaxResult> Remove(string jobIds)
        {
            var idList = jobIds.SplitToList<long>();
            var data = await _sysJobService.DeleteAsync(idList);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 导出 定时任务调度
        /// </summary>
        [HttpPost("export")]
        [AppAuthorize("monitor:job:export")]
        [Log(Title = "定时任务", BusinessType = BusinessType.EXPORT)]
        public async Task Export(SysJobDto dto)
        {
            var dtos = await _sysJobService.GetDtoListAsync(dto);
            await ExcelUtils.ExportAsync(App.HttpContext.Response, dtos, sheetName: "定时任务");
        }
    }

}
