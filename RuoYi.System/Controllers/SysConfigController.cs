using RuoYi.Common.Enums;
using RuoYi.Common.Utils;
using RuoYi.Data.Dtos;
using RuoYi.Framework;
using RuoYi.System.Services;

namespace RuoYi.System.Controllers
{
    /// <summary>
    /// 参数配置表
    /// </summary>
    [ApiDescriptionSettings("System")]
    [Route("system/config")]
    public class SysConfigController : ControllerBase
    {
        private readonly ILogger<SysConfigController> _logger;
        private readonly SysConfigService _sysConfigService;
                
        public SysConfigController(ILogger<SysConfigController> logger,
            SysConfigService sysConfigService)
        {
            _logger = logger;
            _sysConfigService = sysConfigService;
        }

        /// <summary>
        /// 查询参数配置表列表
        /// </summary>
        [HttpGet("list")]
        [AppAuthorize("system:config:list")]
        public async Task<SqlSugarPagedList<SysConfigDto>> GetSysConfigList([FromQuery] SysConfigDto dto)
        {
           return await _sysConfigService.GetDtoPagedListAsync(dto);
        }

        /// <summary>
        /// 获取 参数配置表 详细信息
        /// </summary>
        [HttpGet("{id}")]
        [AppAuthorize("system:config:query")]
        public async Task<AjaxResult> Get(int id)
        {
            var data = await _sysConfigService.GetAsync(id);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 根据键名查询参数配置信息
        /// </summary>
        /// <param name="configKey">参数键名</param>
        [HttpGet("configKey/{configKey}")]
        public AjaxResult GetConfigKey(string configKey)
        {
            var data = _sysConfigService.SelectConfigByKey(configKey);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 新增 参数配置表
        /// </summary>
        [HttpPost("")]
        [AppAuthorize("system:config:add")]
        [TypeFilter(typeof(RuoYi.Framework.DataValidation.DataValidationFilter))]
        [Log(Title = "参数管理", BusinessType = BusinessType.INSERT)]
        public async Task<AjaxResult> Add([FromBody] SysConfigDto dto)
        {
            if (!_sysConfigService.CheckConfigKeyUnique(dto))
            {
                return AjaxResult.Error("新增参数'" + dto.ConfigName + "'失败，参数键名已存在");
            }
            await _sysConfigService.InsertConfigAsync(dto);
            return AjaxResult.Success();
        }

        /// <summary>
        /// 修改 参数配置表
        /// </summary>
        [HttpPut("")]
        [AppAuthorize("system:config:edit")]
        [TypeFilter(typeof(RuoYi.Framework.DataValidation.DataValidationFilter))]
        [Log(Title = "参数管理", BusinessType = BusinessType.UPDATE)]
        public async Task<AjaxResult> Edit([FromBody] SysConfigDto dto)
        {
            if (!_sysConfigService.CheckConfigKeyUnique(dto))
            {
                return AjaxResult.Error("修改参数'" + dto.ConfigName + "'失败，参数键名已存在");
            }
            var data = await _sysConfigService.UpdateConfigAsync(dto);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 删除 参数配置表
        /// </summary>
        [HttpDelete("{configIds}")]
        [AppAuthorize("system:config:remove")]
        [Log(Title = "参数管理", BusinessType = BusinessType.DELETE)]
        public async Task<AjaxResult> Remove(int[] configIds)
        {
            await _sysConfigService.DeleteConfigByIdsAsync(configIds);
            return AjaxResult.Success();
        }

        /// <summary>
        /// 导出 参数配置表
        /// </summary>
        [HttpPost("export")]
        [AppAuthorize("system:config:export")]
        [Log(Title = "参数管理", BusinessType = BusinessType.EXPORT)]
        public async Task Export(SysConfigDto dto)
        {
            var list = await _sysConfigService.GetDtoListAsync(dto);
            await ExcelUtils.ExportAsync(App.HttpContext.Response, list);
        }

        /// <summary>
        /// 刷新参数缓存
        /// </summary>
        [HttpDelete("refreshCache")]
        [AppAuthorize("system:config:remove")]
        [Log(Title = "参数管理", BusinessType = BusinessType.CLEAN)]
        public AjaxResult RefreshCache()
        {
            _sysConfigService.ResetConfigCache();
            return AjaxResult.Success();
        }
    }
}