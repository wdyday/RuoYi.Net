namespace RuoYi.System.Repositories;

/// <summary>
///  参数配置表 Repository
///  author ruoyi
///  date   2023-08-21 14:40:22
/// </summary>
public class SysConfigRepository : BaseRepository<SysConfig, SysConfigDto>
{
    public SysConfigRepository(ISqlSugarRepository<SysConfig> sqlSugarRepository)
    {
        Repo = sqlSugarRepository;
    }

    public override ISugarQueryable<SysConfig> Queryable(SysConfigDto dto)
    {
        return Repo.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(dto.ConfigType), (c) => c.ConfigType == dto.ConfigType)
            .WhereIF(!string.IsNullOrEmpty(dto.ConfigKey), (c) => c.ConfigKey!.Contains(dto.ConfigKey!))
            .WhereIF(!string.IsNullOrEmpty(dto.ConfigName), (c) => c.ConfigName!.Contains(dto.ConfigKey!))
            .WhereIF(dto.Params.BeginTime != null, (u) => u.CreateTime >= dto.Params.BeginTime)
            .WhereIF(dto.Params.EndTime != null, (u) => u.CreateTime <= dto.Params.EndTime)
            .WhereIF(!string.IsNullOrEmpty(dto.ConfigKey), (c) => c.ConfigKey == dto.ConfigKey)
        ;
    }

    public override ISugarQueryable<SysConfigDto> DtoQueryable(SysConfigDto dto)
    {
        return Repo.AsQueryable()
            .WhereIF(!string.IsNullOrEmpty(dto.ConfigType), (c) => c.ConfigType == dto.ConfigType)
            .WhereIF(!string.IsNullOrEmpty(dto.ConfigKey), (c) => c.ConfigKey!.Contains(dto.ConfigKey!))
            .WhereIF(!string.IsNullOrEmpty(dto.ConfigName), (c) => c.ConfigName!.Contains(dto.ConfigName!))
            .WhereIF(dto.Params.BeginTime != null, (u) => u.CreateTime >= dto.Params.BeginTime)
            .WhereIF(dto.Params.EndTime != null, (u) => u.CreateTime <= dto.Params.EndTime)
            .Select((c) => new SysConfigDto
            {
                CreateBy = c.CreateBy,
                CreateTime = c.CreateTime,
                UpdateBy = c.UpdateBy,
                UpdateTime = c.UpdateTime,

                ConfigId = c.ConfigId,
                ConfigName = c.ConfigName,
                ConfigKey = c.ConfigKey,
                ConfigValue = c.ConfigValue,
                ConfigType = c.ConfigType,
                Remark = c.Remark
            });
    }

    // dtos 关联表数据
    protected override async Task FillRelatedDataAsync(IEnumerable<SysConfigDto> dtos)
    {
        await base.FillRelatedDataAsync(dtos);

        if (dtos.IsEmpty()) return;

        // 关联表处理
        foreach (var dto in dtos)
        {
            dto.ConfigTypeDesc = YesNo.ToDesc(dto.ConfigType);
        }
    }

    public SysConfig CheckConfigKeyUnique(string configKey)
    {
        return base.FirstOrDefault(c => c.ConfigKey == configKey);
    }
}