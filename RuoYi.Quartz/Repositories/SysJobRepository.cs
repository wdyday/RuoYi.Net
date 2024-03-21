using RuoYi.Quartz.Enums;

namespace RuoYi.Quartz.Repositories;

/// <summary>
///  定时任务调度表 Repository
///  author ruoyi.net
///  date   2023-10-12 17:31:30
/// </summary>
public class SysJobRepository : BaseRepository<SysJob, SysJobDto>
{
    public SysJobRepository(ISqlSugarRepository<SysJob> sqlSugarRepository)
    {
        Repo = sqlSugarRepository;
    }

    public override ISugarQueryable<SysJob> Queryable(SysJobDto dto)
    {
        return Repo.AsQueryable()
            .WhereIF(dto.JobId > 0, (t) => t.JobId == dto.JobId)
            .WhereIF(!string.IsNullOrEmpty(dto.JobName), (t) => t.JobName.Contains(dto.JobName!))
            .WhereIF(!string.IsNullOrEmpty(dto.InvokeTarget), (t) => t.InvokeTarget.Contains(dto.InvokeTarget!))
            .WhereIF(!string.IsNullOrEmpty(dto.JobGroup), (t) => t.JobName == dto.JobName)
            .WhereIF(!string.IsNullOrEmpty(dto.Status), (t) => t.Status == dto.Status)
        ;
    }

    public override ISugarQueryable<SysJobDto> DtoQueryable(SysJobDto dto)
    {
        return Repo.AsQueryable()
            .WhereIF(dto.JobId > 0, (t) => t.JobId == dto.JobId)
            .WhereIF(!string.IsNullOrEmpty(dto.JobName), (t) => t.JobName.Contains(dto.JobName!))
            .WhereIF(!string.IsNullOrEmpty(dto.InvokeTarget), (t) => t.InvokeTarget.Contains(dto.InvokeTarget!))
            .WhereIF(!string.IsNullOrEmpty(dto.JobGroup), (t) => t.JobName == dto.JobName)
            .WhereIF(!string.IsNullOrEmpty(dto.Status), (t) => t.Status == dto.Status)
            .Select((t) => new SysJobDto
            {
                JobId = t.JobId
            }, true);
    }

    protected override async Task FillRelatedDataAsync(IEnumerable<SysJobDto> dtos)
    {
        await base.FillRelatedDataAsync(dtos);

        foreach (var dto in dtos)
        {
            // 计划执行错误策略（0=默认,1=触发立即执行,2=触发一次执行,3=不触发立即执行）
            dto.MisfirePolicyDesc = !string.IsNullOrEmpty(dto.MisfirePolicy) ? Enum.Parse<JobMisfirePolicy>(dto.MisfirePolicy!).ToDesc() : null;
            // 是否并发执行（0允许 1禁止)
            dto.ConcurrentDesc = !string.IsNullOrEmpty(dto.Concurrent) ? Enum.Parse<ConcurrentStatus>(dto.Concurrent!).ToDesc() : null;
            // 状态（0正常 1暂停）
            dto.StatusDesc = !string.IsNullOrEmpty(dto.Status) ? Enum.Parse<ScheduleStatus>(dto.Status!).ToDesc() : null;
        }
    }
}