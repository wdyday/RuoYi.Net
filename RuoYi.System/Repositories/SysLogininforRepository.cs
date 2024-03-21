namespace RuoYi.System.Repositories;

/// <summary>
///  系统访问记录 Repository
///  author ruoyi
///  date   2023-08-22 10:07:36
/// </summary>
public class SysLogininforRepository : BaseRepository<SysLogininfor, SysLogininforDto>
{
    public SysLogininforRepository(ISqlSugarRepository<SysLogininfor> sqlSugarRepository)
    {
        Repo = sqlSugarRepository;
    }

    public override ISugarQueryable<SysLogininfor> Queryable(SysLogininforDto dto)
    {
        return Repo.AsQueryable()
            .WhereIF(dto.InfoId > 0, (t) => t.InfoId == dto.InfoId)
            .WhereIF(!string.IsNullOrEmpty(dto.Ipaddr), (t) => t.Ipaddr!.Contains(dto.Ipaddr!))
            .WhereIF(!string.IsNullOrEmpty(dto.UserName), (t) => t.Ipaddr!.Contains(dto.UserName!))
            .WhereIF(!string.IsNullOrEmpty(dto.Status), (t) => t.Status == dto.Status)
            .WhereIF(dto.Params.BeginTime != null, (u) => u.LoginTime >= dto.Params.BeginTime)
            .WhereIF(dto.Params.EndTime != null, (u) => u.LoginTime <= dto.Params.EndTime)
        ;
    }

    public override ISugarQueryable<SysLogininforDto> DtoQueryable(SysLogininforDto dto)
    {
        return Repo.AsQueryable()
            .WhereIF(dto.InfoId > 0, (t) => t.InfoId == dto.InfoId)
            .WhereIF(!string.IsNullOrEmpty(dto.Ipaddr), (t) => t.Ipaddr!.Contains(dto.Ipaddr!))
            .WhereIF(!string.IsNullOrEmpty(dto.UserName), (t) => t.Ipaddr!.Contains(dto.UserName!))
            .WhereIF(!string.IsNullOrEmpty(dto.Status), (t) => t.Status == dto.Status)
            .WhereIF(dto.Params.BeginTime != null, (u) => u.LoginTime >= dto.Params.BeginTime)
            .WhereIF(dto.Params.EndTime != null, (u) => u.LoginTime <= dto.Params.EndTime)
            .Select((t) => new SysLogininforDto
            {
                InfoId = t.InfoId,
                UserName = t.UserName,
                Ipaddr = t.Ipaddr,
                LoginLocation = t.LoginLocation,
                Browser = t.Browser,
                Os = t.Os,
                Status = t.Status,
                Msg = t.Msg,
                LoginTime = t.LoginTime
            });
    }
}