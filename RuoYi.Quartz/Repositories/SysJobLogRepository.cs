using RuoYi.Common.Enums;

namespace RuoYi.Quartz.Repositories;

/// <summary>
///  定时任务调度日志表 Repository
///  author ruoyi.net
///  date   2023-10-12 17:38:31
/// </summary>
public class SysJobLogRepository : BaseRepository<SysJobLog, SysJobLogDto>
{
    public SysJobLogRepository(ISqlSugarRepository<SysJobLog> sqlSugarRepository)
    {
        Repo = sqlSugarRepository;
    }

    public override ISugarQueryable<SysJobLog> Queryable(SysJobLogDto dto)
    {
        return Repo.AsQueryable()
            .WhereIF(dto.JobLogId > 0, (t) => t.JobLogId == dto.JobLogId)
            .WhereIF(dto.Params.BeginTime != null, (t) => t.CreateTime >= dto.Params.BeginTime)
            .WhereIF(dto.Params.EndTime != null, (t) => t.CreateTime <= dto.Params.EndTime)
            .WhereIF(!string.IsNullOrEmpty(dto.JobName), (d) => d.JobName!.Contains(dto.JobName!))
            .WhereIF(!string.IsNullOrEmpty(dto.JobGroup), (d) => d.JobGroup == dto.JobGroup)
            .WhereIF(!string.IsNullOrEmpty(dto.Status), (d) => d.Status == dto.Status)
            .WhereIF(!string.IsNullOrEmpty(dto.InvokeTarget), (d) => d.InvokeTarget!.Contains(dto.InvokeTarget!))
        ;
    }

    public override ISugarQueryable<SysJobLogDto> DtoQueryable(SysJobLogDto dto)
    {
        return Repo.AsQueryable()
            .WhereIF(dto.JobLogId > 0, (t) => t.JobLogId == dto.JobLogId)
            .WhereIF(dto.Params.BeginTime != null, (t) => t.CreateTime >= dto.Params.BeginTime)
            .WhereIF(dto.Params.EndTime != null, (t) => t.CreateTime <= dto.Params.EndTime)
            .WhereIF(!string.IsNullOrEmpty(dto.JobName), (d) => d.JobName!.Contains(dto.JobName!))
            .WhereIF(!string.IsNullOrEmpty(dto.JobGroup), (d) => d.JobGroup == dto.JobGroup)
            .WhereIF(!string.IsNullOrEmpty(dto.Status), (d) => d.Status == dto.Status)
            .WhereIF(!string.IsNullOrEmpty(dto.InvokeTarget), (d) => d.InvokeTarget!.Contains(dto.InvokeTarget!))
            .Select((t) => new SysJobLogDto
            {
                JobLogId = t.JobLogId,
            }, true);
    }

    protected override async Task FillRelatedDataAsync(IEnumerable<SysJobLogDto> dtos)
    {
        await base.FillRelatedDataAsync(dtos);

        foreach (var d in dtos)
        {
            d.StatusDesc = Status.ToDesc(d.Status);
        }
    }

    public void Truncate()
    {
        Repo.Context.DbMaintenance.TruncateTable<SysJobLog>();
    }
}