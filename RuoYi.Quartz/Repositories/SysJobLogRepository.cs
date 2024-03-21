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
        ;
    }

    public override ISugarQueryable<SysJobLogDto> DtoQueryable(SysJobLogDto dto)
    {
        return Repo.AsQueryable()
            .WhereIF(dto.JobLogId > 0, (t) => t.JobLogId == dto.JobLogId)
            .Select((t) => new SysJobLogDto
            {
                JobLogId = t.JobLogId
            }, true);
    }
}