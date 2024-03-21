using RuoYi.Generator.RepoSql;

namespace RuoYi.Generator.Repositories;

public class GenTableColumnRepository : BaseRepository<GenTableColumn, GenTableColumnDto>
{
    private readonly RepoSqlService _repoSqlService;

    public GenTableColumnRepository(ISqlSugarRepository<GenTableColumn> sqlSugarRepository, RepoSqlService repoSqlService)
    {
        Repo = sqlSugarRepository;
        _repoSqlService = repoSqlService;
    }

    public override ISugarQueryable<GenTableColumn> Queryable(GenTableColumnDto dto)
    {
        return Repo.AsQueryable()
            .WhereIF(dto.TableId > 0, (e) => e.TableId == dto.TableId)
            //.WhereIF(!string.IsNullOrEmpty(dto.TableName), (e) => e.TableName.Contains(dto.TableName))
            //.WhereIF(!string.IsNullOrEmpty(dto.TableComment), (e) => e.TableComment.Contains(dto.TableComment))
            ;
    }

    public override ISugarQueryable<GenTableColumnDto> DtoQueryable(GenTableColumnDto dto)
    {
        return Repo.AsQueryable()
            .WhereIF(dto.TableId > 0, (e) => e.TableId == dto.TableId)
            .Select((c) => new GenTableColumnDto
            {
                CreateBy = c.CreateBy,
                CreateTime = c.CreateTime,
                UpdateBy = c.UpdateBy,
                UpdateTime = c.UpdateTime,

                ColumnId = c.ColumnId,
                TableId = c.TableId,
                ColumnName = c.ColumnName,
                ColumnComment = c.ColumnComment,
                ColumnType = c.ColumnType,
                NetType = c.NetType,
                NetField = c.NetField,
                IsPk = c.IsPk,
                IsIncrement = c.IsIncrement,
                IsRequired = c.IsRequired,
                IsInsert = c.IsInsert,
                IsEdit = c.IsEdit,
                IsList = c.IsList,
                IsQuery = c.IsQuery,
                QueryType = c.QueryType,
                HtmlType = c.HtmlType,
                DictType = c.DictType,
                Sort = c.Sort
            });
    }

    #region DbTable

    public List<GenTableColumn> SelectDbTableColumnsByName(string tableName)
    {
        var sql = _repoSqlService.GetDbTableColumnsByName();
        var parameters = new List<SugarParameter>(){
            new SugarParameter("@tableName",$"{tableName}")
        };
        return base.SqlQueryable(sql, parameters).ToList();
    }

    /// <summary>
    /// 新增多条记录
    /// </summary>
    /// <param name="entities"></param>
    public void InsertEntities(IEnumerable<GenTableColumn> entities)
    {
        foreach (var entity in entities)
        {
            entity.CreateTime = DateTime.Now;
            base.Insertable(entity)
                //.IgnoreColumns(c => new { c.Remark })
                .ExecuteCommandIdentityIntoEntity();
        }
    }
    #endregion
}
