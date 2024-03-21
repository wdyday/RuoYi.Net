using RuoYi.Common.Enums;

namespace RuoYi.System.Repositories;

/// <summary>
///  操作日志记录 Repository
///  author ruoyi
///  date   2023-09-28 12:38:39
/// </summary>
public class SysOperLogRepository : BaseRepository<SysOperLog, SysOperLogDto>
{
    public SysOperLogRepository(ISqlSugarRepository<SysOperLog> sqlSugarRepository)
    {
        Repo = sqlSugarRepository;
    }

    public override ISugarQueryable<SysOperLog> Queryable(SysOperLogDto dto)
    {
        return Repo.AsQueryable()
            .WhereIF(dto.OperId > 0, (t) => t.OperId == dto.OperId)
            .WhereIF(dto.Params.BeginTime != null, (u) => u.OperTime >= dto.Params.BeginTime)
            .WhereIF(dto.Params.EndTime != null, (u) => u.OperTime <= dto.Params.EndTime)
            .WhereIF(dto.OperId > 0, (t) => t.OperId == dto.OperId)
        ;
    }

    public override ISugarQueryable<SysOperLogDto> DtoQueryable(SysOperLogDto dto)
    {
        return Repo.AsQueryable()
            .WhereIF(dto.OperId > 0, (t) => t.OperId == dto.OperId)
            .WhereIF(dto.Params.BeginTime != null, (u) => u.OperTime >= dto.Params.BeginTime)
            .WhereIF(dto.Params.EndTime != null, (u) => u.OperTime <= dto.Params.EndTime)
            .Select((t) => new SysOperLogDto
            {
                OperId = t.OperId
            }, true);
    }

    protected override async Task FillRelatedDataAsync(IEnumerable<SysOperLogDto> dtos)
    {
        await base.FillRelatedDataAsync(dtos);

        foreach (var d in dtos)
        {
            d.BusinessTypeDesc = d.BusinessType.HasValue ? ((BusinessType)d.BusinessType).ToDesc(): "";
        }
    }

    public void Truncate()
    {
        Repo.Context.DbMaintenance.TruncateTable<SysOperLog>();
    }
}