using RuoYi.System.Repositories;

namespace RuoYi.System.Services;

/// <summary>
///  角色和菜单关联表 Service
///  author ruoyi.net
///  date   2023-08-23 09:43:53
/// </summary>
public class SysRoleMenuService : BaseService<SysRoleMenu, SysRoleMenuDto>, ITransient
{
    private readonly ILogger<SysRoleMenuService> _logger;
    private readonly SysRoleMenuRepository _sysRoleMenuRepository;

    public SysRoleMenuService(ILogger<SysRoleMenuService> logger,
        SysRoleMenuRepository sysRoleMenuRepository)
    {
        _logger = logger;
        _sysRoleMenuRepository = sysRoleMenuRepository;
        BaseRepo = sysRoleMenuRepository;
    }

    /// <summary>
    /// 查询 角色和菜单关联表 详情
    /// </summary>
    public async Task<SysRoleMenuDto> GetAsync(long? id)
    {
        var entity = await base.FirstOrDefaultAsync(e => e.RoleId == id);
        var dto = entity.Adapt<SysRoleMenuDto>();
        // TODO 填充关联表数据
        return dto;
    }
}