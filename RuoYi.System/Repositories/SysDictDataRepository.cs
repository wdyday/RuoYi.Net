namespace RuoYi.System.Repositories;

/// <summary>
///  字典数据表 Repository
///  author ruoyi
///  date   2023-09-04 17:49:59
/// </summary>
public class SysDictDataRepository : BaseRepository<SysDictData, SysDictDataDto>
{
    public SysDictDataRepository(ISqlSugarRepository<SysDictData> sqlSugarRepository)
    {
        Repo = sqlSugarRepository;
    }

    public override ISugarQueryable<SysDictData> Queryable(SysDictDataDto dto)
    {
        return Repo.AsQueryable()
            //.Includes(t => t.SubTable)
            .WhereIF(dto.DictCode > 0, (t) => t.DictCode == dto.DictCode)
            .WhereIF(!string.IsNullOrEmpty(dto.DictType), (t) => t.DictType == dto.DictType)
            .WhereIF(!string.IsNullOrEmpty(dto.DictLabel), (t) => t.DictLabel!.Contains(dto.DictLabel!))
            .WhereIF(!string.IsNullOrEmpty(dto.Status), (t) => t.Status == dto.Status)
        ;
    }

    public override ISugarQueryable<SysDictDataDto> DtoQueryable(SysDictDataDto dto)
    {
        return Repo.AsQueryable()
            //.LeftJoin<SubTable>((t, s) => t.Id == s.Id)
            .WhereIF(dto.DictCode > 0, (t) => t.DictCode == dto.DictCode)
            .WhereIF(!string.IsNullOrEmpty(dto.DictType), (t) => t.DictType == dto.DictType)
            .WhereIF(!string.IsNullOrEmpty(dto.DictLabel), (t) => t.DictLabel!.Contains(dto.DictLabel!))
            .WhereIF(!string.IsNullOrEmpty(dto.Status), (t) => t.Status == dto.Status)
            .Select((d) => new SysDictDataDto
            {
                CreateBy = d.CreateBy,
                CreateTime = d.CreateTime,
                UpdateBy = d.UpdateBy,
                UpdateTime = d.UpdateTime,

                DictCode = d.DictCode,
                DictSort = d.DictSort,
                DictLabel = d.DictLabel,
                DictValue = d.DictValue,
                DictType = d.DictType,
                CssClass = d.CssClass,
                ListClass = d.ListClass,
                IsDefault = d.IsDefault,
                Status = d.Status,
                Remark = d.Remark,
            });
    }

    protected override async Task FillRelatedDataAsync(IEnumerable<SysDictDataDto> dtos)
    {
        await base.FillRelatedDataAsync(dtos);

        foreach (var dto in dtos)
        {
            dto.StatusDesc = Status.ToDesc(dto.Status);
            dto.IsDefaultDesc = YesNo.ToDesc(dto.IsDefault);
        }
    }

    public async Task<List<SysDictData>> SelectDictDataByTypeAsync(string dictType)
    {
        if (string.IsNullOrEmpty(dictType))
            return null!;

        var query = new SysDictDataDto { DictType = dictType };
        return await base.GetListAsync(query);
    }

    /// <summary>
    /// 同步修改字典类型
    /// </summary>
    /// <param name="oldDictType">旧字典类型</param>
    /// <param name="newDictType">新旧字典类型</param>
    /// <returns></returns>
    public async Task<int> UpdateDictDataTypeAsync(string oldDictType, string newDictType)
    {
        return await base.Updateable()
              .SetColumns(col => col.DictType == newDictType)
              .Where(col => col.DictType == oldDictType)
              .ExecuteCommandAsync();
    }

    /// <summary>
    /// 查询字典数据
    /// </summary>
    /// <param name="dictType">字典类型</param>
    /// <returns></returns>
    public async Task<int> CountDictDataByTypeAsync(string dictType)
    {
        return await base.CountAsync(c => c.DictType == dictType);
    }
}