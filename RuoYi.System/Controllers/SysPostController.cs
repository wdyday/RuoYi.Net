using RuoYi.Common.Enums;
using RuoYi.Common.Utils;
using RuoYi.Data.Dtos;
using RuoYi.Framework;
using RuoYi.Framework.Exceptions;
using RuoYi.System.Services;

namespace RuoYi.System.Controllers
{
    /// <summary>
    /// 岗位信息表
    /// </summary>
    [ApiDescriptionSettings("System")]
    [Route("system/post")]
    public class SysPostController : ControllerBase
    {
        private readonly ILogger<SysPostController> _logger;
        private readonly SysPostService _sysPostService;

        public SysPostController(ILogger<SysPostController> logger,
            SysPostService sysPostService)
        {
            _logger = logger;
            _sysPostService = sysPostService;
        }

        /// <summary>
        /// 查询岗位信息表列表
        /// </summary>
        [HttpGet("list")]
        [AppAuthorize("system:post:list")]
        public async Task<SqlSugarPagedList<SysPostDto>> GetSysPostList([FromQuery] SysPostDto dto)
        {
            return await _sysPostService.GetDtoPagedListAsync(dto);
        }

        /// <summary>
        /// 获取 岗位信息表 详细信息
        /// </summary>
        [HttpGet("{id}")]
        [AppAuthorize("system:post:query")]
        public async Task<AjaxResult> Get(long? id)
        {
            var data = await _sysPostService.GetDtoAsync(id);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 新增 岗位信息表
        /// </summary>
        [HttpPost("")]
        [AppAuthorize("system:post:add")]
        [TypeFilter(typeof(RuoYi.Framework.DataValidation.DataValidationFilter))]
        [Log(Title = "岗位管理", BusinessType = BusinessType.INSERT)]
        public async Task<AjaxResult> Add([FromBody] SysPostDto post)
        {
            if (!await _sysPostService.CheckPostNameUniqueAsync(post))
            {
                throw new ServiceException("新增岗位'" + post.PostName + "'失败，岗位名称已存在");
            }
            else if (!await _sysPostService.CheckPostCodeUniqueAsync(post))
            {
                throw new ServiceException("新增岗位'" + post.PostName + "'失败，岗位编码已存在");
            }
            var data = await _sysPostService.InsertAsync(post);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 修改 岗位信息表
        /// </summary>
        [HttpPut("")]
        [AppAuthorize("system:post:edit")]
        [TypeFilter(typeof(RuoYi.Framework.DataValidation.DataValidationFilter))]
        [Log(Title = "岗位管理", BusinessType = BusinessType.UPDATE)]
        public async Task<AjaxResult> Edit([FromBody] SysPostDto post)
        {
            if (!await _sysPostService.CheckPostNameUniqueAsync(post))
            {
                throw new ServiceException("修改岗位'" + post.PostName + "'失败，岗位名称已存在");
            }
            else if (!await _sysPostService.CheckPostCodeUniqueAsync(post))
            {
                throw new ServiceException("修改岗位'" + post.PostName + "'失败，岗位编码已存在");
            }
            var data = await _sysPostService.UpdateAsync(post);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 删除 岗位信息表
        /// </summary>
        [HttpDelete("{ids}")]
        [AppAuthorize("system:post:remove")]
        [Log(Title = "岗位管理", BusinessType = BusinessType.DELETE)]
        public async Task<AjaxResult> Remove(long[] ids)
        {
            var data = await _sysPostService.DeleteAsync(ids);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 导出 岗位信息表
        /// </summary>
        [HttpPost("export")]
        [AppAuthorize("system:post:export")]
        [Log(Title = "岗位管理", BusinessType = BusinessType.EXPORT)]
        public async Task Export(SysPostDto dto)
        {
            var list = await _sysPostService.GetDtoListAsync(dto);
            await ExcelUtils.ExportAsync(App.HttpContext.Response, list);
        }

        /// <summary>
        /// 获取岗位选择框列表
        /// </summary>
        [HttpGet("optionselect")]
        public async Task<AjaxResult> OptionSelect()
        {
            var data = await _sysPostService.SelectPostAllAsync();
            return AjaxResult.Success(data);
        }
    }
}