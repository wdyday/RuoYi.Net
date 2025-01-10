using RuoYi.Common.Enums;
using RuoYi.Common.Utils;
using RuoYi.Data.Dtos;
using RuoYi.Framework;
using RuoYi.System.Services;

namespace RuoYi.System.Controllers
{
    /// <summary>
    /// 字典类型表
    /// </summary>
    [ApiDescriptionSettings("System")]
    [Route("system/dict/type")]
    public class SysDictTypeController : ControllerBase
    {
        private readonly ILogger<SysDictTypeController> _logger;
        private readonly SysDictTypeService _sysDictTypeService;

        public SysDictTypeController(ILogger<SysDictTypeController> logger,
            SysDictTypeService sysDictTypeService)
        {
            _logger = logger;
            _sysDictTypeService = sysDictTypeService;
        }

        /// <summary>
        /// 查询字典类型表列表
        /// </summary>
        [HttpGet("list")]
        [AppAuthorize("system:dict:list")]
        public async Task<SqlSugarPagedList<SysDictTypeDto>> GetSysDictTypeList([FromQuery] SysDictTypeDto dto)
        {
            return await _sysDictTypeService.GetDtoPagedListAsync(dto);
        }

        /// <summary>
        /// 获取 字典类型表 详细信息
        /// </summary>
        [HttpGet("{id}")]
        [AppAuthorize("system:dict:query")]
        public async Task<AjaxResult> Get(long? id)
        {
            var data = await _sysDictTypeService.GetAsync(id);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 新增 字典类型表
        /// </summary>
        [HttpPost("")]
        [AppAuthorize("system:dict:add")]
        [TypeFilter(typeof(RuoYi.Framework.DataValidation.DataValidationFilter))]
        [Log(Title = "字典类型", BusinessType = BusinessType.INSERT)]
        public async Task<AjaxResult> Add([FromBody] SysDictTypeDto dto)
        {
            if (!await _sysDictTypeService.CheckDictTypeUniqueAsync(dto))
            {
                return AjaxResult.Error("新增字典'" + dto.DictName + "'失败，字典类型已存在");
            }
            var data = await _sysDictTypeService.InsertDictTypeAsync(dto);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 修改 字典类型表
        /// </summary>
        [HttpPut("")]
        [AppAuthorize("system:dict:edit")]
        [TypeFilter(typeof(RuoYi.Framework.DataValidation.DataValidationFilter))]
        [Log(Title = "字典类型", BusinessType = BusinessType.UPDATE)]
        public async Task<AjaxResult> Edit([FromBody] SysDictTypeDto dto)
        {
            if (!await _sysDictTypeService.CheckDictTypeUniqueAsync(dto))
            {
                return AjaxResult.Error("修改字典'" + dto.DictName + "'失败，字典类型已存在");
            }
            var data = await _sysDictTypeService.UpdateDictTypeAsync(dto);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 删除 字典类型表
        /// </summary>
        [HttpDelete("{ids}")]
        [AppAuthorize("system:dict:remove")]
        [Log(Title = "字典类型", BusinessType = BusinessType.DELETE)]
        public async Task<AjaxResult> Remove(long[] ids)
        {
            await _sysDictTypeService.DeleteDictTypeByIdsAsync(ids);
            return AjaxResult.Success();
        }

        /// <summary>
        /// 刷新字典缓存
        /// </summary>
        [HttpDelete("refreshCache")]
        [AppAuthorize("system:dict:remove")]
        [Log(Title = "字典类型", BusinessType = BusinessType.CLEAN)]
        public AjaxResult RefreshCache()
        {
            _sysDictTypeService.ResetDictCache();
            return AjaxResult.Success();
        }

        /// <summary>
        /// 获取字典选择框列表
        /// </summary>
        [HttpGet("optionselect")]
        public async Task<AjaxResult> OptionSelect()
        {
            var data = await _sysDictTypeService.SelectDictTypeAllAsync();
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 导出 字典类型表
        /// </summary>
        [HttpPost("export")]
        [AppAuthorize("system:dict:export")]
        [Log(Title = "字典类型", BusinessType = BusinessType.EXPORT)]
        public async Task Export(SysDictTypeDto dto)
        {
            var list = await _sysDictTypeService.GetDtoListAsync(dto);
            await ExcelUtils.ExportAsync(App.HttpContext.Response, list);
        }
    }
}