using RuoYi.System.Repositories;

namespace RuoYi.System.Services;

/// <summary>
///  操作日志记录 Service
///  author ruoyi
///  date   2023-09-28 12:38:39
/// </summary>
public class SysOperLogService : BaseService<SysOperLog, SysOperLogDto>, ITransient
{
    private readonly ILogger<SysOperLogService> _logger;
    private readonly SysOperLogRepository _sysOperLogRepository;

    public SysOperLogService(ILogger<SysOperLogService> logger,
        SysOperLogRepository sysOperLogRepository)
    {
        BaseRepo = sysOperLogRepository;

        _logger = logger;
        _sysOperLogRepository = sysOperLogRepository;
    }

    /// <summary>
    /// 查询 操作日志记录 详情
    /// </summary>
    public async Task<SysOperLog> GetAsync(long id)
    {
        var entity = await base.FirstOrDefaultAsync(e => e.OperId == id);
        return entity;
    }

    /// <summary>
    /// 查询 操作日志记录 详情
    /// </summary>
    public async Task<SysOperLogDto> GetDtoAsync(long id)
    {
        var entity = await base.FirstOrDefaultAsync(e => e.OperId == id);
        var dto = entity.Adapt<SysOperLogDto>();
        // TODO 填充关联表数据
        return dto;
    }

    /// <summary>
    /// 清表
    /// </summary>
    public void Clean()
    {
        _sysOperLogRepository.Truncate();
    }
}