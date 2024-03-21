namespace RuoYi.System.Repositories;

/// <summary>
///  字典类型表 Repository
///  author ruoyi
///  date   2023-09-04 17:49:59
/// </summary>
public class SysDictTypeRepository : BaseRepository<SysDictType, SysDictTypeDto>
{
    public SysDictTypeRepository(ISqlSugarRepository<SysDictType> sqlSugarRepository)
    {
        Repo = sqlSugarRepository;
    }

    public override ISugarQueryable<SysDictType> Queryable(SysDictTypeDto dto)
    {
        return Repo.AsQueryable()
            .WhereIF(dto.DictId > 0, (t) => t.DictId == dto.DictId)
            .WhereIF(!string.IsNullOrEmpty(dto.Status), (t) => t.Status == dto.Status)
            .WhereIF(!string.IsNullOrEmpty(dto.DictType), (t) => t.DictType!.Contains(dto.DictType!))
            .WhereIF(!string.IsNullOrEmpty(dto.DictName), (t) => t.DictName!.Contains(dto.DictName!))
            .WhereIF(dto.Params.BeginTime != null, (t) => t.CreateTime >= dto.Params.BeginTime)
            .WhereIF(dto.Params.EndTime != null, (t) => t.CreateTime <= dto.Params.EndTime)
        ;
    }

    public override ISugarQueryable<SysDictTypeDto> DtoQueryable(SysDictTypeDto dto)
    {
        return Repo.AsQueryable()
            .WhereIF(dto.DictId > 0, (t) => t.DictId == dto.DictId)
            .WhereIF(!string.IsNullOrEmpty(dto.Status), (t) => t.Status == dto.Status)
            .WhereIF(!string.IsNullOrEmpty(dto.DictType), (t) => t.DictType!.Contains(dto.DictType!))
            .WhereIF(!string.IsNullOrEmpty(dto.DictName), (t) => t.DictName!.Contains(dto.DictName!))
            .WhereIF(dto.Params.BeginTime != null, (t) => t.CreateTime >= dto.Params.BeginTime)
            .WhereIF(dto.Params.EndTime != null, (t) => t.CreateTime <= dto.Params.EndTime)
            .Select((t) => new SysDictTypeDto
            {
                CreateBy = t.CreateBy,
                CreateTime = t.CreateTime,
                UpdateBy = t.UpdateBy,
                UpdateTime = t.UpdateTime,

                DictId = t.DictId,
                DictType = t.DictType,
                DictName = t.DictName,
                Status = t.Status,
                Remark = t.Remark
            });
    }

    protected override async Task FillRelatedDataAsync(IEnumerable<SysDictTypeDto> dtos)
    {
        await base.FillRelatedDataAsync(dtos);

        foreach (var dto in dtos)
        {
            dto.StatusDesc = Status.ToDesc(dto.Status);
        }
    }

    public async Task<List<SysDictType>> SelectDictDataByTypeAsync(string dictType)
    {
        if (string.IsNullOrEmpty(dictType))
            return null!;

        var query = new SysDictTypeDto { DictType = dictType };
        return await base.GetListAsync(query);
    }

    /// <summary>
    /// 校验字典类型称是否唯一
    /// </summary>
    /// <param name="dictType">字典类型</param>
    public async Task<SysDictType> CheckDictTypeUniqueAsync(string dictType)
    {
        if (string.IsNullOrEmpty(dictType))
            return null!;

        var query = new SysDictTypeDto { DictType = dictType };
        return await base.GetFirstAsync(query);
    }
}