using RuoYi.System.Repositories;

namespace RuoYi.System.Services;

/// <summary>
///  角色和部门关联表 Service
///  author ruoyi.net
///  date   2023-08-23 09:43:52
/// </summary>
public class SysRoleDeptService : BaseService<SysRoleDept, SysRoleDeptDto>, ITransient
{
    private readonly ILogger<SysRoleDeptService> _logger;
    private readonly SysRoleDeptRepository _sysRoleDeptRepository;

    public SysRoleDeptService(ILogger<SysRoleDeptService> logger,
        SysRoleDeptRepository sysRoleDeptRepository)
    {
        _logger = logger;
        _sysRoleDeptRepository = sysRoleDeptRepository;
        BaseRepo = sysRoleDeptRepository;
    }

    /// <summary>
    /// 查询 角色和部门关联表 详情
    /// </summary>
    public async Task<SysRoleDeptDto> GetAsync(long? id)
    {
        var entity = await base.FirstOrDefaultAsync(e => e.RoleId == id);
        var dto = entity.Adapt<SysRoleDeptDto>();
        // TODO 填充关联表数据
        return dto;
    }
}