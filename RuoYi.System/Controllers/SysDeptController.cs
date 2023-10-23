using RuoYi.Common.Constants;
using RuoYi.Common.Enums;
using RuoYi.Data.Dtos;
using RuoYi.Framework;
using RuoYi.System.Services;

namespace RuoYi.System.Controllers
{
    /// <summary>
    /// 部门表
    /// </summary>
    [ApiDescriptionSettings("System")]
    [Route("system/dept")]
    public class SysDeptController : ControllerBase
    {
        private readonly ILogger<SysDeptController> _logger;
        private readonly SysDeptService _sysDeptService;

        public SysDeptController(ILogger<SysDeptController> logger,
            SysDeptService sysDeptService)
        {
            _logger = logger;
            _sysDeptService = sysDeptService;
        }

        /// <summary>
        /// 查询部门表列表
        /// </summary>
        [HttpGet("list")]
        [AppAuthorize("system:dept:list")]
        public async Task<AjaxResult> GetSysDeptList([FromQuery] SysDeptDto dto)
        {
            var data = await _sysDeptService.GetDtoListAsync(dto);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 查询部门表列表
        /// </summary>
        [HttpGet("list/exclude/{deptId}")]
        [AppAuthorize("system:dept:list")]
        public async Task<AjaxResult> ExcludeChildList(long? deptId)
        {
            var list = await _sysDeptService.GetDtoListAsync(new SysDeptDto());
            var id = deptId ?? 0;
            var data = list.Where(d => d.DeptId != id || (!d.Ancestors?.Split(",").Contains(id.ToString()) ?? false)).ToList();
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 获取 部门表 详细信息
        /// </summary>
        [HttpGet("{deptId}")]
        [AppAuthorize("system:dept:query")]
        public async Task<AjaxResult> Get(long deptId)
        {
            await _sysDeptService.CheckDeptDataScopeAsync(deptId);
            var data = await _sysDeptService.GetDtoAsync(deptId);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 新增 部门表
        /// </summary>
        [HttpPost("")]
        [AppAuthorize("system:dept:add")]
        [TypeFilter(typeof(RuoYi.Framework.DataValidation.DataValidationFilter))]
        [Log(Title = "部门管理", BusinessType = BusinessType.INSERT)]
        public async Task<AjaxResult> Add([FromBody] SysDeptDto dept)
        {
            if (!await _sysDeptService.CheckDeptNameUniqueAsync(dept))
            {
                return AjaxResult.Error($"新增部门'{dept.DeptName} '失败，部门名称已存在");
            }
            var data = await _sysDeptService.InsertDeptAsync(dept);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 修改 部门表
        /// </summary>
        [HttpPut("")]
        [AppAuthorize("system:dept:edit")]
        [TypeFilter(typeof(RuoYi.Framework.DataValidation.DataValidationFilter))]
        [Log(Title = "部门管理", BusinessType = BusinessType.UPDATE)]
        public async Task<AjaxResult> Edit([FromBody] SysDeptDto dept)
        {
            long deptId = dept.DeptId!.Value;
            await _sysDeptService.CheckDeptDataScopeAsync(deptId);
            if (!await _sysDeptService.CheckDeptNameUniqueAsync(dept))
            {
                return AjaxResult.Error("修改部门'" + dept.DeptName + "'失败，部门名称已存在");
            }
            else if (dept.ParentId.Equals(deptId))
            {
                return AjaxResult.Error("修改部门'" + dept.DeptName + "'失败，上级部门不能是自己");
            }
            else if (UserConstants.DEPT_DISABLE.Equals(dept.Status) && await _sysDeptService.CountNormalChildrenDeptByIdAsync(deptId) > 0)
            {
                return AjaxResult.Error("该部门包含未停用的子部门！");
            }
            var data = await _sysDeptService.UpdateDeptAsync(dept);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 删除 部门表
        /// </summary>
        [HttpDelete("{deptId}")]
        [AppAuthorize("system:dept:remove")]
        [Log(Title = "部门管理", BusinessType = BusinessType.DELETE)]
        public async Task<AjaxResult> Remove(long deptId)
        {
            if (await _sysDeptService.HasChildByDeptIdAsync(deptId))
            {
                return AjaxResult.Error("存在下级部门,不允许删除");
            }
            if (await _sysDeptService.CheckDeptExistUserAsync(deptId))
            {
                return AjaxResult.Error("部门存在用户,不允许删除");
            }
            await _sysDeptService.CheckDeptDataScopeAsync(deptId);
            var data = await _sysDeptService.DeleteDeptByIdAsync(deptId);
            return AjaxResult.Success(data);
        }
    }
}