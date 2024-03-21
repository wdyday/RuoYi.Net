using RuoYi.Framework.JsonSerialization;
using RuoYi.Generator.RepoSql;

namespace RuoYi.Generator.Repositories;

public class GenTableRepository : BaseRepository<GenTable, GenTableDto>
{
    private readonly RepoSqlService _repoSqlService;

    public GenTableRepository(ISqlSugarRepository<GenTable> sqlSugarRepository, RepoSqlService repoSqlService)
    {
        Repo = sqlSugarRepository;
        _repoSqlService = repoSqlService;
    }

    public override ISugarQueryable<GenTable> Queryable(GenTableDto dto)
    {
        return Repo.AsQueryable()
            .Includes(t => t.Columns)
            .WhereIF(dto.TableId > 0, (e) => e.TableId == dto.TableId)
            .WhereIF(!string.IsNullOrEmpty(dto.TableName), (e) => e.TableName!.Contains(dto.TableName!))
            .WhereIF(!string.IsNullOrEmpty(dto.TableComment), (e) => e.TableComment!.Contains(dto.TableComment!))
            .WhereIF(dto.Params.BeginTime != null, (u) => u.CreateTime >= dto.Params.BeginTime)
            .WhereIF(dto.Params.EndTime != null, (u) => u.CreateTime < dto.Params.EndTime!.Value.Date.AddDays(1))
            ;
    }

    public override ISugarQueryable<GenTableDto> DtoQueryable(GenTableDto dto)
    {
        return Repo.AsQueryable()
            .LeftJoin<GenTableColumn>((t, c) => t.TableId == c.TableId)
            .WhereIF(dto.TableId > 0, (e) => e.TableId == dto.TableId)
            .WhereIF(!string.IsNullOrEmpty(dto.TableName), (e) => e.TableName!.Contains(dto.TableName!))
            .WhereIF(!string.IsNullOrEmpty(dto.TableComment), (e) => e.TableComment!.Contains(dto.TableComment!))
            .WhereIF(dto.Params.EndTime != null, (u) => u.CreateTime < dto.Params.EndTime!.Value.Date.AddDays(1))
            .Select((t, c) => new GenTableDto
            {
                CreateBy = t.CreateBy,
                CreateTime = t.CreateTime,
                UpdateBy = t.UpdateBy,
                UpdateTime = t.UpdateTime,

                TableId = t.TableId,
                TableName = t.TableName,
                TableComment = t.TableComment,
                SubTableName = t.SubTableName,
                SubTableFkName = t.SubTableFkName,
                ClassName = t.ClassName,
                TplCategory = t.TplCategory,
                PackageName = t.PackageName,
                ModuleName = t.ModuleName,
                BusinessName = t.BusinessName,
                FunctionName = t.FunctionName,
                FunctionAuthor = t.FunctionAuthor,
                GenType = t.GenType,
                GenPath = t.GenPath,
                Options = t.Options,
            });
    }

    protected override async Task FillRelatedDataAsync(IEnumerable<GenTableDto> dtos)
    {
        await base.FillRelatedDataAsync(dtos);

        this.SetOptions(dtos);
    }

    public List<GenTableDto> ToDtos(IEnumerable<GenTable> genTables)
    {
        if (genTables == null) return null;

        var dtos = genTables.Adapt<List<GenTableDto>>();
        this.SetOptions(dtos);

        return dtos;
    }

    public GenTableDto ToDto(GenTable genTable)
    {
        var dto = genTable.Adapt<GenTableDto>();
        return this.SetOptions(dto);
    }

    private IEnumerable<GenTableDto> SetOptions(IEnumerable<GenTableDto> dtos)
    {
        foreach (var dto in dtos)
        {
            this.SetOptions(dto);
        }
        return dtos;
    }

    private GenTableDto SetOptions(GenTableDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Options))
        {
            var options = JSON.Deserialize<GenTableOptions>(dto.Options);

            dto.TreeCode = options.TreeCode;
            dto.TreeName = options.TreeName;
            dto.TreeParentCode = options.TreeParentCode;
            dto.ParentMenuId = options.ParentMenuId;
            dto.ParentMenuName = options.ParentMenuName;

        }
        return dto;
    }

    public List<GenTable> SelectGenTableAll()
    {
        return Repo.AsQueryable().Includes(t => t.Columns).ToList();
    }

    public GenTable SelectGenTableByName(string tableName)
    {
        var param = new GenTableDto { TableName = tableName };
        return Queryable(param).First();
    }

    public GenTable SelectGenTableById(long tableId)
    {
        var param = new GenTableDto { TableId = tableId };
        return Queryable(param).First();
    }

    #region DbTable

    public ISugarQueryable<GenTable> DbQueryable(GenTableDto dto)
    {
        var sp = _repoSqlService.GetDbTableListSqlAndParameter(dto);
        return base.SqlQueryable(sp.Sql, sp.Parameters);
    }

    public List<GenTable> SelectDbTableListByNames(string[] tableNames)
    {
        var sql = _repoSqlService.GetDbTableListByNamesSql();
        var parameters = new List<SugarParameter>(){
            new SugarParameter("@tableNames", tableNames)
        };

        return Repo.Ado.SqlQuery<GenTable>(sql, parameters);
    }

    #endregion
}
