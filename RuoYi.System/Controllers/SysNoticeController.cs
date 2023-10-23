using RuoYi.Common.Enums;
using RuoYi.Data.Dtos;
using RuoYi.Framework;
using RuoYi.System.Services;

namespace RuoYi.System.Controllers
{
    /// <summary>
    /// 通知公告表
    /// </summary>
    [ApiDescriptionSettings("System")]
    [Route("system/notice")]
    public class SysNoticeController : ControllerBase
    {
        private readonly ILogger<SysNoticeController> _logger;
        private readonly SysNoticeService _sysNoticeService;

        public SysNoticeController(ILogger<SysNoticeController> logger,
            SysNoticeService sysNoticeService)
        {
            _logger = logger;
            _sysNoticeService = sysNoticeService;
        }

        /// <summary>
        /// 查询通知公告表列表
        /// </summary>
        [HttpGet("list")]
        [AppAuthorize("system:notice:list")]
        public async Task<SqlSugarPagedList<SysNoticeDto>> GetSysNoticeList([FromQuery] SysNoticeDto dto)
        {
            return await _sysNoticeService.GetDtoPagedListAsync(dto);
        }
        /// <summary>
        /// 获取 通知公告表 详细信息
        /// </summary>
        [HttpGet("{id}")]
        [AppAuthorize("system:notice:query")]
        public async Task<AjaxResult> Get(int id)
        {
            var data = await _sysNoticeService.GetDtoAsync(id);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 新增 通知公告表
        /// </summary>
        [HttpPost("")]
        [AppAuthorize("system:notice:add")]
        [TypeFilter(typeof(RuoYi.Framework.DataValidation.DataValidationFilter))]
        [Log(Title = "通知公告", BusinessType = BusinessType.INSERT)]
        public async Task<AjaxResult> Add([FromBody] SysNoticeDto dto)
        {
            var data = await _sysNoticeService.InsertAsync(dto);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 修改 通知公告表
        /// </summary>
        [HttpPut("")]
        [AppAuthorize("system:notice:edit")]
        [TypeFilter(typeof(RuoYi.Framework.DataValidation.DataValidationFilter))]
        [Log(Title = "通知公告", BusinessType = BusinessType.UPDATE)]
        public async Task<AjaxResult> Edit([FromBody] SysNoticeDto dto)
        {
            var data = await _sysNoticeService.UpdateAsync(dto);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 删除 通知公告表
        /// </summary>
        [HttpDelete("{ids}")]
        [AppAuthorize("system:notice:remove")]
        [Log(Title = "通知公告", BusinessType = BusinessType.DELETE)]
        public async Task<AjaxResult> Remove(long[] ids)
        {
            var data = await _sysNoticeService.DeleteAsync(ids);
            return AjaxResult.Success(data);
        }
    }
}