using Furion.DependencyInjection;
using Mapster;
using Microsoft.AspNetCore.Http;
using RuoYi.Data.Dtos;
using RuoYi.Data.Entities;
using RuoYi.Data.Utils;
using RuoYi.Framework.Extensions;
using RuoYi.Framework.Utils;
using SqlSugar;
using System.Linq.Expressions;

namespace RuoYi.Data
{
    public abstract class BaseRepository<TEntity, TDto> : ITransient
        where TEntity : BaseEntity, new()
        where TDto : BaseDto, new()
    {
        public virtual ISqlSugarRepository<TEntity> Repo { get; set; }
        public abstract ISugarQueryable<TEntity> Queryable(TDto dto);
        public abstract ISugarQueryable<TDto> DtoQueryable(TDto dto);
        protected virtual async Task FillRelatedDataAsync(IEnumerable<TDto> dtos)
        {
            await Task.FromResult(true);
        }
        //public virtual async Task FillRelatedDataAsync(IEnumerable<TEntity> entities)
        //{
        //    await Task.FromResult(true);
        //}

        public IUpdateable<TEntity> Updateable()
        {
            if (typeof(TEntity) == typeof(UserBaseEntity))
            {
                return Repo.Context.Updateable<TEntity>()
                    .SetColumns("update_time", DateTime.Now)
                    .SetColumns("update_by", SecurityUtils.GetUsername());
            }
            else
            {
                return Repo.Context.Updateable<TEntity>();
            }
        }

        public IUpdateable<TEntity> Updateable(TEntity entity)
        {
            this.SetUpdateUserInfo(entity);
            return Repo.Context.Updateable<TEntity>(entity);
        }

        public IUpdateable<TEntity> Updateable(IEnumerable<TEntity> entities)
        {
            this.SetUpdateUserInfo(entities);
            return Repo.Context.Updateable<TEntity>(entities);
        }

        public IInsertable<TEntity> Insertable(TEntity entity)
        {
            this.SetCreateUserInfo(entity);
            return Repo.Context.Insertable<TEntity>(entity);
        }

        public IInsertable<TEntity> Insertable(IEnumerable<TEntity> entities)
        {
            this.SetCreateUserInfo(entities);
            return Repo.Context.Insertable<TEntity>(entities.ToList());
        }

        public ISugarQueryable<TEntity> SqlQueryable(string sql)
        {
            return Repo.SqlQueryable(sql);
        }

        public ISugarQueryable<TEntity> SqlQueryable(string sql, List<SugarParameter> parameters)
        {
            return Repo.SqlQueryable(sql, parameters);
        }

        #region Select

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return this.FirstOrDefaultAsync(predicate).GetAwaiter().GetResult();
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return this.CountAsync(predicate).GetAwaiter().GetResult();
        }

        public int Count(TDto dto)
        {
            return this.CountAsync(dto).GetAwaiter().GetResult();
        }

        public bool Any(TDto dto)
        {
            return this.AnyAsync(dto).GetAwaiter().GetResult();
        }

        public TEntity GetFirst(TDto dto)
        {
            return this.GetFirstAsync(dto).GetAwaiter().GetResult();
        }

        public List<TEntity> GetList(TDto dto)
        {
            return this.GetListAsync(dto).GetAwaiter().GetResult();
        }

        // 分页查询
        public SqlSugarPagedList<TEntity> GetPagedList(TDto dto)
        {
            var quryable = Queryable(dto);
            return this.GetPagedList(quryable);
        }

        // 分页查询
        public SqlSugarPagedList<TEntity> GetPagedList(ISugarQueryable<TEntity> queryable)
        {
            return this.GetPagedListAsync(queryable).GetAwaiter().GetResult();
        }

        #region 返回 Dto
        public List<TDto> GetDtoList(TDto dto)
        {
            return this.GetDtoListAsync(dto).GetAwaiter().GetResult();
        }

        // 分页查询
        public SqlSugarPagedList<TDto> GetDtoPagedList(TDto dto)
        {
            var queryable = DtoQueryable(dto);
            return this.GetDtoPagedList(queryable);
        }

        public SqlSugarPagedList<TDto> GetDtoPagedList(ISugarQueryable<TDto> queryable)
        {
            return this.GetDtoPagedListAsync(queryable).GetAwaiter().GetResult();
        }
        #endregion

        #region Async

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Repo.FirstOrDefaultAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Repo.CountAsync(predicate);
        }

        public async Task<int> CountAsync(TDto dto)
        {
            return await Queryable(dto).CountAsync();
        }

        public async Task<bool> AnyAsync(TDto dto)
        {
            return await Queryable(dto).AnyAsync();
        }

        public async Task<TEntity> GetFirstAsync(TDto dto)
        {
            var entities = await GetListAsync(dto);
            return entities.FirstOrDefault()!;
        }

        public async Task<List<TEntity>> GetListAsync(TDto dto)
        {
            var entities = await Queryable(dto).ToListAsync();
            //// 填充关联表数据
            //if (entities.IsNotEmpty())
            //{
            //    await FillRelatedDataAsync(entities);
            //}
            return entities;
        }

        // 分页查询
        public async Task<SqlSugarPagedList<TEntity>> GetPagedListAsync(TDto dto)
        {
            var queryable = Queryable(dto);
            return await this.GetPagedListAsync(queryable);
        }

        // 分页查询
        public async Task<SqlSugarPagedList<TEntity>> GetPagedListAsync(ISugarQueryable<TEntity> queryable)
        {
            var pageDomain = PageUtils.GetPageDomain();

            SqlSugarPagedList<TEntity> pagedInfo;
            if (!string.IsNullOrEmpty(pageDomain.PropertyName))
            {
                OrderByType? orderByType = (pageDomain.IsAsc ?? "").EqualsIgnoreCase("desc") ? OrderByType.Desc : OrderByType.Asc;
                pagedInfo = await queryable
                    .OrderByPropertyName(pageDomain.PropertyName, orderByType)
                    .ToPagedListAsync(pageDomain.PageNum, pageDomain.PageSize);
            }
            else
            {
                pagedInfo = await queryable.ToPagedListAsync(pageDomain.PageNum, pageDomain.PageSize);
            }

            //// 填充关联表数据
            //if (pagedInfo.Rows.IsNotEmpty())
            //{
            //    await FillRelatedDataAsync(pagedInfo.Rows);
            //}

            pagedInfo.Code = StatusCodes.Status200OK;

            return pagedInfo;
        }

        #region 返回 Dto

        public async Task<TDto> GetDtoFirstAsync(TDto dto)
        {
            var entities = await GetDtoListAsync(dto);
            return entities.FirstOrDefault()!;
        }

        public async Task<List<TDto>> GetDtoListAsync(TDto dto)
        {
            var dtos = await DtoQueryable(dto).ToListAsync();

            // 填充关联表数据
            if (dtos.IsNotEmpty())
            {
                await FillRelatedDataAsync(dtos);
            }

            return dtos;
        }

        public async Task<SqlSugarPagedList<TDto>> GetDtoPagedListAsync(TDto dto)
        {
            var queryable = DtoQueryable(dto);
            var pagedInfo = await this.GetDtoPagedListAsync(queryable);

            // 填充关联表数据
            if (pagedInfo.Rows.IsNotEmpty())
            {
                await FillRelatedDataAsync(pagedInfo.Rows);
            }

            return pagedInfo;
        }

        public async Task<SqlSugarPagedList<TDto>> GetDtoPagedListAsync(ISugarQueryable<TDto> queryable)
        {
            var pageDomain = PageUtils.GetPageDomain();

            SqlSugarPagedList<TDto> pagedInfo;
            if (!string.IsNullOrEmpty(pageDomain.PropertyName))
            {
                OrderByType? orderByType = (pageDomain.IsAsc ?? "").EqualsIgnoreCase("desc") ? OrderByType.Desc : OrderByType.Asc;
                pagedInfo = await queryable
                    .OrderByPropertyName(pageDomain.PropertyName, orderByType)
                    .ToPagedListAsync(pageDomain.PageNum, pageDomain.PageSize);
            }
            else
            {
                pagedInfo = await queryable.ToPagedListAsync(pageDomain.PageNum, pageDomain.PageSize);
            }
            pagedInfo.Code = StatusCodes.Status200OK;

            // 填充关联表数据
            if (pagedInfo.Rows.IsNotEmpty())
            {
                await FillRelatedDataAsync(pagedInfo.Rows);
            }

            return pagedInfo;
        }
        #endregion

        #endregion

        #endregion

        #region Insert

        public bool Insert(TEntity entity)
        {
            return this.InsertAsync(entity).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 新增多条记录
        /// </summary>
        /// <param name="entities"></param>
        public int Insert(IEnumerable<TEntity> entities)
        {
            return this.InsertAsync(entities).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 批量插入
        /// </summary>
        public int InsertBatch(IEnumerable<TEntity> entities)
        {
            this.SetCreateUserInfo(entities);
            return Repo.Context.Fastest<TEntity>().BulkCopy(entities.ToList());
        }

        public bool Insert(TDto dto)
        {
            var entity = dto.Adapt<TEntity>();
            var success = this.Insert(entity);
            SetDtoPrimaryKeyValue(dto, entity);
            return success;
        }

        /// <summary>
        /// 新增多条记录
        /// </summary>
        /// <param name="dtos"></param>
        public int Insert(IEnumerable<TDto> dtos)
        {
            var entities = dtos.Adapt<List<TEntity>>();
            return this.Insert(entities);
        }

        public async Task<bool> InsertAsync(TEntity entity)
        {
            this.SetCreateUserInfo(entity);
            return await Repo.InsertReturnIndentityIntoEntityAsync(entity);
        }

        public async Task<int> InsertAsync(IEnumerable<TEntity> entities)
        {
            this.SetCreateUserInfo(entities);
            return await Repo.InsertAsync(entities);
        }

        /// <summary>
        /// 批量插入, 不能使用事务
        /// </summary>
        public async Task<int> InsertBatchAsync(IEnumerable<TEntity> entities)
        {
            this.SetCreateUserInfo(entities);
            return await Repo.Context.Fastest<TEntity>().BulkCopyAsync(entities.ToList());
        }

        public async Task<bool> InsertAsync(TDto dto)
        {
            var entity = dto.Adapt<TEntity>();
            var success = await this.InsertAsync(entity);
            SetDtoPrimaryKeyValue(dto, entity);

            return success;
        }

        #endregion

        #region Update

        public int Update(TEntity entity, bool ignoreAllNullColumns = false)
        {
            return this.UpdateAsync(entity, ignoreAllNullColumns).GetAwaiter().GetResult();
        }

        public int Update(IEnumerable<TEntity> entities)
        {
            return this.UpdateAsync(entities).GetAwaiter().GetResult();
        }

        public async Task<int> UpdateAsync(TEntity entity, bool ignoreAllNullColumns = false)
        {
            this.SetUpdateUserInfo(entity);
            return await Repo.UpdateAsync(entity, ignoreAllNullColumns);
        }

        public async Task<int> UpdateAsync(IEnumerable<TEntity> entities)
        {
            this.SetUpdateUserInfo(entities);
            return await Repo.UpdateAsync(entities);
        }

        public int Update(TDto dto, bool ignoreAllNullColumns = false)
        {
            var entity = dto.Adapt<TEntity>();
            return this.Update(entity, ignoreAllNullColumns);
        }

        public async Task<int> UpdateAsync(TDto dto, bool ignoreAllNullColumns = false)
        {
            var entity = dto.Adapt<TEntity>();
            return await this.UpdateAsync(entity, ignoreAllNullColumns);
        }

        public int Update(IEnumerable<TDto> dtos)
        {
            var entities = dtos.Adapt<List<TEntity>>();
            return this.Update(entities);
        }

        public async Task<int> UpdateAsync(IEnumerable<TDto> dtos)
        {
            var entities = dtos.Adapt<List<TEntity>>();
            return await this.UpdateAsync(entities);
        }

        #endregion

        #region Delete

        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="entity"></param>
        public int Delete(TEntity entity)
        {
            return this.DeleteAsync(entity).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="dto"></param>
        public int Delete(TDto dto)
        {
            var entity = dto.Adapt<TEntity>();
            return this.DeleteAsync(entity).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="key"></param>
        public int Delete<TKey>(TKey key)
        {
            return this.DeleteAsync(key).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 删除多条记录
        /// </summary>
        /// <param name="keys"></param>
        public int Delete<TKey>(TKey[] keys)
        {
            return this.DeleteAsync(keys).GetAwaiter().GetResult();
        }

        public int Delete<TKey>(List<TKey> keys)
        {
            return this.DeleteAsync(keys).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 自定义条件删除记录
        /// </summary>
        /// <param name="whereExpression"></param>
        public int Delete(Expression<Func<TEntity, bool>> whereExpression)
        {
            return this.DeleteAsync(whereExpression).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="entity"></param>
        public async Task<int> DeleteAsync(TEntity entity)
        {
            return await Repo.DeleteAsync(entity);
        }

        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="dto"></param>
        public async Task<int> DeleteAsync(TDto dto)
        {
            var entity = dto.Adapt<TEntity>();
            return await Repo.DeleteAsync(entity);
        }

        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="key"></param>
        public async Task<int> DeleteAsync<TKey>(TKey key)
        {
            return await Repo.DeleteAsync(key);
        }

        /// <summary>
        /// 删除多条记录
        /// </summary>
        /// <param name="keys"></param>
        public async Task<int> DeleteAsync<TKey>(TKey[] keys)
        {
            return await Repo.DeleteAsync(keys);
        }

        /// <summary>
        /// 删除多条记录
        /// </summary>
        /// <param name="keys"></param>
        public async Task<int> DeleteAsync<TKey>(List<TKey> keys)
        {
            return await Repo.DeleteAsync(keys);
        }

        /// <summary>
        /// 自定义条件删除记录
        /// </summary>
        /// <param name="whereExpression"></param>
        public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> whereExpression)
        {
            return await Repo.DeleteAsync(whereExpression);
        }
        #endregion

        // 将 Entity 的主键值 赋给 Dto 的主键
        private void SetDtoPrimaryKeyValue(TDto dto, TEntity entity)
        {
            object? id = null;
            string keyName = "";
            Type type;
            var props = typeof(TEntity).GetProperties();
            foreach (var prop in props)
            {
                var attrSugarColumn = prop.CustomAttributes.Where(a => a.AttributeType == typeof(SugarColumn)).FirstOrDefault();
                if (attrSugarColumn != null)
                {
                    var primaryKey = attrSugarColumn.NamedArguments.Where(arg => arg.MemberInfo.Name.EqualsIgnoreCase("IsPrimaryKey")).FirstOrDefault();
                    if (primaryKey != null)
                    {
                        keyName = prop.Name;
                        type = prop.PropertyType;
                        id = prop.GetValue(entity);
                        break;
                    }
                }
            }
            if (keyName.IsNotEmpty())
            {
                ReflectUtils.SetPropertyValue(dto, keyName, id);
            }
        }

        // 新增时: 设置 用户信息
        private void SetCreateUserInfo(TEntity entity)
        {
            if (typeof(TEntity).BaseType != typeof(UserBaseEntity)) return;

            ReflectUtils.SetPropertyValue(entity, "CreateBy", SecurityUtils.GetUsername()!);
            ReflectUtils.SetPropertyValue(entity, "CreateTime", DateTime.Now);
        }
        private void SetCreateUserInfo(IEnumerable<TEntity> entities)
        {
            if (typeof(TEntity) != typeof(UserBaseEntity)) return;
            foreach (TEntity entity in entities)
            {
                this.SetCreateUserInfo(entity);
            }
        }

        // 更新时: 设置 用户信息
        private void SetUpdateUserInfo(TEntity entity)
        {
            if (typeof(TEntity).BaseType != typeof(UserBaseEntity)) return;

            ReflectUtils.SetPropertyValue(entity, "UpdateBy", SecurityUtils.GetUsername()!);
            ReflectUtils.SetPropertyValue(entity, "UpdateTime", DateTime.Now);
        }
        private void SetUpdateUserInfo(IEnumerable<TEntity> entities)
        {
            if (typeof(TEntity) != typeof(UserBaseEntity)) return;
            foreach (TEntity entity in entities)
            {
                this.SetUpdateUserInfo(entity);
            }
        }
    }
}
