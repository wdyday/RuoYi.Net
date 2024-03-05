using Microsoft.Extensions.Logging;
using RuoYi.Common.Enums;
using RuoYi.Common.Utils;
using RuoYi.Data.Dtos;
using RuoYi.System.Services;
using SqlSugar;
using System.IO;

namespace RuoYi.System.Controllers
{
    /// <summary>
    /// 系统访问记录
    /// </summary>
    [ApiDescriptionSettings("Monitor")]
    [Route("monitor/logininfor")]
    public class SysLogininforController : ControllerBase
    {
        private readonly ILogger<SysLogininforController> _logger;
        private readonly SysLogininforService _sysLogininforService;
                
        public SysLogininforController(ILogger<SysLogininforController> logger,
            SysLogininforService sysLogininforService)
        {
            _logger = logger;
            _sysLogininforService = sysLogininforService;
        }

        /// <summary>
        /// 查询系统访问记录列表
        /// </summary>
        [HttpGet("list")]
        [AppAuthorize("system:logininfor:list")]
        public async Task<SqlSugarPagedList<SysLogininforDto>> GetSysLogininforList([FromQuery] SysLogininforDto dto)
        {
            return await _sysLogininforService.GetDtoPagedListAsync(dto);
        }

        /// <summary>
        /// 获取 系统访问记录 详细信息
        /// </summary>
        [HttpGet("")]
        [HttpGet("{id}")]
        [AppAuthorize("system:logininfor:query")]
        public async Task<AjaxResult> Get(long id)
        {
            var data = await _sysLogininforService.GetDtoAsync(id);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 新增 系统访问记录
        /// </summary>
        [HttpPost("")]
        [AppAuthorize("system:logininfor:add")]
        [TypeFilter(typeof(RuoYi.Framework.DataValidation.DataValidationFilter))]
        [RuoYi.System.Log(Title = "系统访问记录", BusinessType = BusinessType.INSERT)]
        public async Task<AjaxResult> Add([FromBody] SysLogininforDto dto)
        {
            var data = await _sysLogininforService.InsertAsync(dto);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 修改 系统访问记录
        /// </summary>
        [HttpPut("")]
        [AppAuthorize("system:logininfor:edit")]
        [TypeFilter(typeof(RuoYi.Framework.DataValidation.DataValidationFilter))]
        [RuoYi.System.Log(Title = "系统访问记录", BusinessType = BusinessType.UPDATE)]
        public async Task<AjaxResult> Edit([FromBody] SysLogininforDto dto)
        {
            var data = await _sysLogininforService.UpdateAsync(dto);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 删除 系统访问记录
        /// </summary>
        [HttpDelete("{ids}")]
        [AppAuthorize("system:logininfor:remove")]
        [RuoYi.System.Log(Title = "系统访问记录", BusinessType = BusinessType.DELETE)]
        public async Task<AjaxResult> Remove(string ids)
        {
            var idList = ids.SplitToList<long>();
            var data = await _sysLogininforService.DeleteAsync(idList);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 导入 系统访问记录
        /// </summary>
        [HttpPost("import")]
        [AppAuthorize("system:logininfor:import")]
        [RuoYi.System.Log(Title = "系统访问记录", BusinessType = BusinessType.IMPORT)]
        public async Task Import([Required] IFormFile file)
        {
            var stream = new MemoryStream();
            file.CopyTo(stream);
            var list = await ExcelUtils.ImportAsync<SysLogininforDto>(stream);
            await _sysLogininforService.ImportDtoBatchAsync(list);
        }

        /// <summary>
        /// 导出 系统访问记录
        /// </summary>
        [HttpPost("export")]
        [AppAuthorize("system:logininfor:export")]
        [RuoYi.System.Log(Title = "系统访问记录", BusinessType = BusinessType.EXPORT)]
        public async Task Export(SysLogininforDto dto)
        {
            var list = await _sysLogininforService.GetDtoListAsync(dto);
            await ExcelUtils.ExportAsync(App.HttpContext.Response, list);
        }
    }
}