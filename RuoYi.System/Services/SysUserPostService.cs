using RuoYi.System.Repositories;

namespace RuoYi.System.Services;

/// <summary>
///  用户与岗位关联表 Service
///  author ruoyi.net
///  date   2023-08-23 09:43:50
/// </summary>
public class SysUserPostService : BaseService<SysUserPost, SysUserPostDto>, ITransient
{
    private readonly ILogger<SysUserPostService> _logger;
    private readonly SysUserPostRepository _sysUserPostRepository;

    public SysUserPostService(ILogger<SysUserPostService> logger,
        SysUserPostRepository sysUserPostRepository)
    {
        _logger = logger;
        _sysUserPostRepository = sysUserPostRepository;
        BaseRepo = sysUserPostRepository;
    }

    /// <summary>
    /// 查询 用户与岗位关联表 详情
    /// </summary>
    public async Task<SysUserPostDto> GetAsync(long? id)
    {
        var entity = await base.FirstOrDefaultAsync(e => e.UserId == id);
        var dto = entity.Adapt<SysUserPostDto>();
        return dto;
    }
}