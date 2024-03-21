using RuoYi.Quartz.Repositories;

namespace RuoYi.Quartz.Services;

/// <summary>
///  定时任务调度日志表 Service
///  author ruoyi.net
///  date   2023-10-12 17:38:31
/// </summary>
public class SysJobLogService : BaseService<SysJobLog, SysJobLogDto>, ITransient
{
    private readonly ILogger<SysJobLogService> _logger;
    private readonly SysJobLogRepository _sysJobLogRepository;

    public SysJobLogService(ILogger<SysJobLogService> logger,
        SysJobLogRepository sysJobLogRepository)
    {
        BaseRepo = sysJobLogRepository;

        _logger = logger;
        _sysJobLogRepository = sysJobLogRepository;
    }

    /// <summary>
    /// 查询 定时任务调度日志表 详情
    /// </summary>
    public async Task<SysJobLog> GetAsync(long id)
    {
        var entity = await base.FirstOrDefaultAsync(e => e.JobLogId == id);
        return entity;
    }

    /// <summary>
    /// 查询 定时任务调度日志表 详情
    /// </summary>
    public async Task<SysJobLogDto> GetDtoAsync(long id)
    {
        var entity = await base.FirstOrDefaultAsync(e => e.JobLogId == id);
        var dto = entity.Adapt<SysJobLogDto>();
        // TODO 填充关联表数据
        return dto;
    }
}