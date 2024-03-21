using RuoYi.Common.Data;
using RuoYi.Common.Enums;
using RuoYi.Common.Utils;
using RuoYi.Data.Dtos;
using RuoYi.Data.Entities;
using RuoYi.Framework;
using RuoYi.System.Services;

namespace RuoYi.System.Controllers
{
    /// <summary>
    /// 用户信息表
    /// </summary>
    [ApiDescriptionSettings("System")]
    [Route("system/user")]
    public class SysUserController : ControllerBase
    {
        private readonly ILogger<SysUserController> _logger;
        private readonly SysUserService _sysUserService;
        private readonly SysRoleService _sysRoleService;
        private readonly SysPostService _sysPostService;
        private readonly SysDeptService _sysDeptService;

        public SysUserController(ILogger<SysUserController> logger,
            SysUserService sysUserService, SysRoleService sysRoleService, SysPostService sysPostService, SysDeptService sysDeptService)
        {
            _logger = logger;
            _sysUserService = sysUserService;
            _sysRoleService = sysRoleService;
            _sysPostService = sysPostService;
            _sysDeptService = sysDeptService;
        }

        /// <summary>
        /// 查询用户信息表列表
        /// </summary>
        [HttpGet("list")]
        [AppAuthorize("system:user:list")]
        public async Task<SqlSugarPagedList<SysUser>> GetUserList([FromQuery] SysUserDto dto)
        {
            return await _sysUserService.GetPagedUserListAsync(dto);
        }

        /// <summary>
        /// 获取 用户信息表 详细信息
        /// </summary>
        [HttpGet("")]
        [HttpGet("{userId}")]
        [AppAuthorize("system:user:query")]
        public async Task<AjaxResult> GetInfo(long? userId)
        {
            await _sysUserService.CheckUserDataScope(userId);
            var roles = await _sysRoleService.GetListAsync(new SysRoleDto());
            var posts = await _sysPostService.GetListAsync(new SysPostDto());

            AjaxResult ajax = AjaxResult.Success();
            ajax.Add("roles", SecurityUtils.IsAdmin(userId) ? roles : roles.Where(r => !SecurityUtils.IsAdminRole(r.RoleId)));
            ajax.Add("posts", posts);

            if (userId.HasValue && userId > 0)
            {
                var user = await _sysUserService.GetDtoAsync(userId);
                ajax.Add(AjaxResult.DATA_TAG, user);
                ajax.Add("postIds", _sysPostService.GetPostIdsListByUserId(userId.Value));
                ajax.Add("roleIds", user.Roles.Select(x => x.RoleId).ToList());
            }

            return ajax;
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        [HttpPost("")]
        [AppAuthorize("system:user:add")]
        [TypeFilter(typeof(RuoYi.Framework.DataValidation.DataValidationFilter))]
        [Log(Title = "用户管理", BusinessType = BusinessType.INSERT)]
        public async Task<AjaxResult> Add([FromBody] SysUserDto user)
        {
            if (!await _sysUserService.CheckUserNameUniqueAsync(user))
            {
                return AjaxResult.Error("新增用户'" + user.UserName + "'失败，登录账号已存在");
            }
            else if (!string.IsNullOrEmpty(user.Phonenumber) && !await _sysUserService.CheckPhoneUniqueAsync(user))
            {
                return AjaxResult.Error("新增用户'" + user.UserName + "'失败，手机号码已存在");
            }
            else if (!string.IsNullOrEmpty(user.Email) && !await _sysUserService.CheckEmailUniqueAsync(user))
            {
                return AjaxResult.Error("新增用户'" + user.UserName + "'失败，邮箱账号已存在");
            }
            var data = _sysUserService.InsertUser(user);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        [HttpPut("")]
        [AppAuthorize("system:user:edit")]
        [TypeFilter(typeof(RuoYi.Framework.DataValidation.DataValidationFilter))]
        [Log(Title = "用户管理", BusinessType = BusinessType.UPDATE)]
        public async Task<AjaxResult> Edit([FromBody] SysUserDto user)
        {
            _sysUserService.CheckUserAllowed(user);
            await _sysUserService.CheckUserDataScope(user.UserId ?? 0);
            if (!await _sysUserService.CheckUserNameUniqueAsync(user))
            {
                return AjaxResult.Error("修改用户'" + user.UserName + "'失败，登录账号已存在");
            }
            else if (!string.IsNullOrEmpty(user.Phonenumber) && !await _sysUserService.CheckPhoneUniqueAsync(user))
            {
                return AjaxResult.Error("修改用户'" + user.UserName + "'失败，手机号码已存在");
            }
            else if (!string.IsNullOrEmpty(user.Email) && !await _sysUserService.CheckEmailUniqueAsync(user))
            {
                return AjaxResult.Error("修改用户'" + user.UserName + "'失败，邮箱账号已存在");
            }
            var data = _sysUserService.UpdateUser(user);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        [HttpDelete("{ids}")]
        [AppAuthorize("system:user:remove")]
        [Log(Title = "用户管理", BusinessType = BusinessType.DELETE)]
        public async Task<AjaxResult> Remove(string ids)
        {
            var userIds = ids.SplitToList<long>();
            if (userIds.Contains(SecurityUtils.GetUserId()))
            {
                return AjaxResult.Error("当前用户不能删除");
            }
            var data = await _sysUserService.DeleteUserByIdsAsync(userIds);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        [HttpPut("resetPwd")]
        [AppAuthorize("system:user:resetPwd")]
        [Log(Title = "用户管理", BusinessType = BusinessType.UPDATE)]
        public async Task<AjaxResult> ResetPwd([FromBody] SysUserDto user)
        {
            _sysUserService.CheckUserAllowed(user);
            await _sysUserService.CheckUserDataScope(user.UserId ?? 0);
            var data = _sysUserService.ResetPwd(user);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 状态修改
        /// </summary>
        [HttpPut("changeStatus")]
        [AppAuthorize("system:user:edit")]
        [Log(Title = "用户管理", BusinessType = BusinessType.UPDATE)]
        public async Task<AjaxResult> ChangeStatus([FromBody] SysUserDto user)
        {
            _sysUserService.CheckUserAllowed(user);
            await _sysUserService.CheckUserDataScope(user.UserId ?? 0);
            var data = await _sysUserService.UpdateUserStatus(user);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 根据用户编号获取授权角色
        /// </summary>
        [HttpGet("authRole/{userId}")]
        [AppAuthorize("system:user:query")]
        public async Task<AjaxResult> GetAuthRole(long userId)
        {
            var user = await _sysUserService.GetDtoAsync(userId);
            var roles = await _sysRoleService.GetRolesByUserIdAsync(userId);

            AjaxResult ajax = AjaxResult.Success();
            ajax.Add("user", user);
            ajax.Add("roles", SecurityUtils.IsAdmin(userId) ? roles : roles.Where(r => !SecurityUtils.IsAdminRole(r.RoleId)));
            return ajax;
        }

        /// <summary>
        /// 用户授权角色
        /// </summary>
        [HttpPut("authRole")]
        [AppAuthorize("system:user:edit")]
        [Log(Title = "用户管理", BusinessType = BusinessType.GRANT)]
        public async Task<AjaxResult> InsertAuthRole(long userId, string roleIds)
        {
            var rIds = roleIds.SplitToList<long>();
            await _sysUserService.CheckUserDataScope(userId);
            _sysUserService.InsertUserAuth(userId, rIds);
            return AjaxResult.Success();
        }

        /// <summary>
        /// 获取部门树列表
        /// </summary>
        /// <param name="dept"></param>
        /// <returns></returns>
        [HttpGet("deptTree")]
        [AppAuthorize("system:user:list")]
        public async Task<AjaxResult> GetDeptTree([FromQuery] SysDeptDto dept)
        {
            var data = await _sysDeptService.GetDeptTreeListAsync(dept);
            return AjaxResult.Success(data);
        }

        /// <summary>
        /// 导入 用户信息表
        /// </summary>
        [HttpPost("importData")]
        [AppAuthorize("system:user:import")]
        [Log(Title = "用户管理", BusinessType = BusinessType.IMPORT)]
        public async Task<AjaxResult> Import([Required] IFormFile file, bool updateSupport)
        {
            var stream = new MemoryStream();
            file.CopyTo(stream);
            var list = await ExcelUtils.ImportAllAsync<SysUserDto>(stream);
            var msg = await _sysUserService.ImportDtosAsync(list, updateSupport, SecurityUtils.GetUsername());
            return AjaxResult.Success(msg);
        }

        /// <summary>
        /// 下载导入模板
        /// </summary>
        /// <returns></returns>
        [HttpPost("importTemplate")]
        [AppAuthorize("system:user:import")]
        public async Task DownloadImportTemplate()
        {
            await ExcelUtils.GetImportTemplateAsync<SysUserDto>(App.HttpContext.Response, "用户数据");
        }

        /// <summary>
        /// 导出 用户信息表
        /// </summary>
        [HttpPost("export")]
        [AppAuthorize("system:user:export")]
        [Log(Title = "用户管理", BusinessType = BusinessType.EXPORT)]
        public async Task Export(SysUserDto dto)
        {
            var list = await _sysUserService.GetUserListAsync(dto);
            var dtos = _sysUserService.ToDtos(list);
            await ExcelUtils.ExportAsync(App.HttpContext.Response, dtos);
        }
    }
}