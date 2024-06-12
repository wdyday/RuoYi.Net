namespace RuoYi.Generator.Services;

/// <summary>
/// 业务字段 服务层
/// </summary>
public class GenTableColumnService : BaseService<GenTableColumn, GenTableColumnDto>, ITransient
{
    private readonly ILogger<GenTableColumnService> _logger;
    private readonly GenTableColumnRepository _genTableColumnRepository;

    public GenTableColumnService(ILogger<GenTableColumnService> logger,
        GenTableColumnRepository genTableColumnRepository)
    {
        _logger = logger;
        _genTableColumnRepository = genTableColumnRepository;
        BaseRepo = genTableColumnRepository;
    }

    /// <summary>
    /// 查询业务字段列表
    /// </summary>
    /// <param name="tableId">业务字段编号</param>
    /// <returns>业务字段集合</returns>
    public List<GenTableColumn> SelectGenTableColumnListByTableId(long tableId)
    {
        var queryable = _genTableColumnRepository.Queryable(new GenTableColumnDto { TableId = tableId });
        return queryable.ToList();
    }

    /**
     * 新增业务字段
     * 
     * @param genTableColumn 业务字段信息
     * @return 结果
     */
    public void InsertGenTableColumn(GenTableColumn genTableColumn)
    {
        _genTableColumnRepository.Insert(genTableColumn);
    }

    /**
     * 修改业务字段
     * 
     * @param genTableColumn 业务字段信息
     * @return 结果
     */
    public int UpdateGenTableColumn(GenTableColumn genTableColumn)
    {
        return _genTableColumnRepository.Update(genTableColumn);
    }

    /**
     * 删除业务字段信息
     * 
     * @param ids 需要删除的数据ID
     * @return 结果
     */
    public int DeleteGenTableColumnByIds(string ids)
    {
        string[] keys = ids.Split(',');
        return _genTableColumnRepository.DeleteByKeys(keys);
    }

    /// <summary>
    /// 查询 业务字段
    /// </summary>
    public async Task<GenTableColumnDto> GetAsync(long id)
    {
        var entity = await base.FirstOrDefaultAsync(e => e.ColumnId == id);
        var dto = entity.Adapt<GenTableColumnDto>();
        return dto;
    }
}
