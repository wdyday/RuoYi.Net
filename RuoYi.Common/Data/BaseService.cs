using RuoYi.Framework.DependencyInjection;
using RuoYi.Framework.Logging;
using Mapster;
using RuoYi.Data.Dtos;
using RuoYi.Data.Entities;
using SqlSugar;
using System.Linq.Expressions;

namespace RuoYi.Common.Data;

public abstract class BaseService<TEntity, TDto> : ITransient
    where TEntity : BaseEntity, new()
    where TDto : BaseDto, new()
{
    public virtual required BaseRepository<TEntity, TDto> BaseRepo { get; set; }

    #region sync

    /// <summary>
    /// 分页查询, 返回 Entity
    /// </summary>
    public virtual SqlSugarPagedList<TEntity> GetPagedList(TDto dto)
    {
        return BaseRepo.GetPagedList(dto);
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    public virtual SqlSugarPagedList<TDto> GetDtoPagedList(TDto dto)
    {
        return BaseRepo.GetDtoPagedList(dto);
    }

    /// <summary>
    /// 查询 List 
    /// </summary>
    public virtual List<TEntity> GetList(TDto dto)
    {
        return BaseRepo.GetList(dto);
    }

    /// <summary>
    /// 查询 Dto List 
    /// </summary>
    public virtual List<TDto> GetDtoList(TDto dto)
    {
        return BaseRepo.GetDtoList(dto);
    }

    /// <summary>
    /// 新增
    /// </summary>
    public virtual bool Insert(TEntity entity)
    {
        return BaseRepo.Insert(entity);
    }

    /// <summary>
    /// 更新
    /// </summary>

    public virtual int Update(TEntity entity)
    {
        return BaseRepo.Update(entity);
    }

    /// <summary>
    /// 按 key 删除
    /// </summary>
    public virtual int Delete(object key)
    {
        return BaseRepo.DeleteByKey(key);
    }

    /// <summary>
    /// 批量新增
    /// </summary>
    public virtual int InsertBatch(IEnumerable<TEntity> entities)
    {
        return BaseRepo.InsertBatch(entities);
    }

    #endregion

    #region async

    /// <summary>
    /// 分页查询, 返回 Entity
    /// </summary>
    public virtual async Task<SqlSugarPagedList<TEntity>> GetPagedListAsync(TDto dto)
    {
        return await BaseRepo.GetPagedListAsync(dto);
    }

    /// <summary>
    /// 分页查询, 返回 Dto
    /// </summary>
    public virtual async Task<SqlSugarPagedList<TDto>> GetDtoPagedListAsync(TDto dto)
    {
        return await BaseRepo.GetDtoPagedListAsync(dto);
    }

    /// <summary>
    /// 查询 List 
    /// </summary>
    public virtual async Task<List<TEntity>> GetListAsync(TDto dto)
    {
        return await BaseRepo.GetListAsync(dto);
    }

    /// <summary>
    /// 查询 Dto List 
    /// </summary>
    public virtual async Task<List<TDto>> GetDtoListAsync(TDto dto)
    {
        return await BaseRepo.GetDtoListAsync(dto);
    }

    /// <summary>
    /// 查询
    /// </summary>
    public virtual async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await BaseRepo.FirstOrDefaultAsync(predicate);
    }

    /// <summary>
    /// 新增
    /// </summary>
    public virtual async Task<bool> InsertAsync(TEntity entity)
    {
        return await BaseRepo.InsertAsync(entity);
    }

    /// <summary>
    /// 更新
    /// </summary>
    public virtual async Task<int> UpdateAsync(TEntity entity)
    {
        return await BaseRepo.UpdateAsync(entity);
    }

    /// <summary>
    /// 按 key 删除
    /// </summary>
    public virtual async Task<int> DeleteAsync(long key)
    {
        return await BaseRepo.DeleteAsync(key);
    }

    /// <summary>
    /// 按 key 删除
    /// </summary>
    public virtual async Task<int> DeleteAsync(long[] keys)
    {
        return await BaseRepo.DeleteAsync(keys);
    }

    /// <summary>
    /// 按 key 删除
    /// </summary>
    public virtual async Task<int> DeleteAsync(List<long> keys)
    {
        return await BaseRepo.DeleteAsync(keys);
    }

    /// <summary>
    /// 批量新增
    /// </summary>
    public virtual async Task<int> InsertBatchAsync(IEnumerable<TEntity> entities)
    {
        return await BaseRepo.InsertBatchAsync(entities);
    }

    /// <summary>
    /// 新增: 按 dto
    /// </summary>
    public virtual async Task<bool> InsertAsync(TDto dto)
    {
        var entity = dto.Adapt<TEntity>();
        return await this.InsertAsync(entity);
    }

    /// <summary>
    /// 更新: 按 dto
    /// </summary>
    public virtual async Task<int> UpdateAsync(TDto dto)
    {
        var entity = dto.Adapt<TEntity>();
        return await this.UpdateAsync(entity);
    }

    #region 导入
    /// <summary>
    /// 批量导入, 默认每次1万条
    /// </summary>
    public virtual async Task ImportEntityBatchAsync(IEnumerable<TEntity> entities, int size = 10000)
    {
        var pageCount = CalculatePageCount(size, entities.Count());
        for (var i = 0; i < pageCount; i++)
        {
            var pageEntities = entities.Skip(i * size).Take(size).ToList();
            try
            {
                await ImportEntitiesAsync(pageEntities);
            }
            catch (Exception ex)
            {
                Log.Error("ImportEntityBatchAsync Error", ex);
            }
        }
    }

    // service 中实现此方法
    public virtual async Task ImportEntitiesAsync(IEnumerable<TEntity> entities)
    {
        await Task.Delay(0);
    }

    /// <summary>
    /// 批量导入, 默认每次1万条
    /// </summary>
    public virtual async Task ImportDtoBatchAsync(IEnumerable<TDto> dtos, int size = 10000)
    {
        var pageCount = CalculatePageCount(size, dtos.Count());
        for (var i = 0; i < pageCount; i++)
        {
            var pageDtos = dtos.Skip(i * size).Take(size).ToList();
            try
            {
                await ImportDtosAsync(pageDtos);
            }
            catch (Exception ex)
            {
                Log.Error("ImportDtoBatchAsync Error", ex);
            }
        }
    }

    // service 中实现此方法
    public virtual async Task ImportDtosAsync(List<TDto> dtos)
    {
        await Task.Delay(0);
    }

    private static int CalculatePageCount(int size, int rowCount)
    {
        return rowCount % size == 0 ? rowCount / size : rowCount / size + 1;
    }

    #endregion

    #endregion
}
