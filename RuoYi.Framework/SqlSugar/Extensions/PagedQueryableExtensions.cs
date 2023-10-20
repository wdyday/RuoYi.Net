namespace SqlSugar;

/// <summary>
/// 分页拓展类
/// </summary>
public static class PagedQueryableExtensions
{
    /// <summary>
    /// 分页拓展
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public static SqlSugarPagedList<TEntity> ToPagedList<TEntity>(this ISugarQueryable<TEntity> entity, int pageIndex, int pageSize)
        where TEntity : new()
    {
        var totalCount = 0;
        var items = entity.ToPageList(pageIndex, pageSize, ref totalCount);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        return new SqlSugarPagedList<TEntity>
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            Rows = items,
            Total = totalCount,
            HasNextPages = pageIndex < totalPages,
            HasPrevPages = pageIndex - 1 > 0
        };
    }

    /// <summary>
    /// 分页拓展
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="pageIndex"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    public static async Task<SqlSugarPagedList<TEntity>> ToPagedListAsync<TEntity>(this ISugarQueryable<TEntity> entity, int pageIndex, int pageSize)
        where TEntity : new()
    {
        RefAsync<int> totalCount = 0;
        var items = await entity.ToPageListAsync(pageIndex, pageSize, totalCount);
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        return new SqlSugarPagedList<TEntity>
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            Rows = items,
            Total = (int)totalCount,
            HasNextPages = pageIndex < totalPages,
            HasPrevPages = pageIndex - 1 > 0
        };
    }
}