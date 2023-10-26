using Furion.JsonSerialization;

namespace RuoYi.Generator.Repositories
{
    public class GenTableRepository : BaseRepository<GenTable, GenTableDto>
    {
        public GenTableRepository(ISqlSugarRepository<GenTable> sqlSugarRepository)
        {
            Repo = sqlSugarRepository;
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

            foreach (var dto in dtos)
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
            }
        }

        public List<GenTable> SelectGenTableAll()
        {
            return Repo.AsQueryable().Includes(t => t.Columns)
            //.LeftJoin<GenTableColumn>((t, c) => t.TableId == c.TableId)
            .ToList();
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
            var dbType = Repo.Context.CurrentConnectionConfig.DbType;
            return dbType switch
            {
                DbType.MySql => GetMySqlDbQueryable(dto),
                DbType.SqlServer => GetSqlServerDbQueryable(dto),
                //DbType.Oracle => GetMySqlDbQueryable(dto),
                _ => GetMySqlDbQueryable(dto),
            };
        }

        public List<GenTable> SelectDbTableListByNames(string[] tableNames)
        {
            var sql = $@"
                SELECT table_name, table_comment, create_time, update_time 
                FROM information_schema.tables
		        WHERE table_schema = (SELECT database())
		        AND table_name NOT LIKE 'qrtz_%' AND table_name NOT LIKE 'gen_%'
		        AND table_name IN (@tableNames)
            ";
            var parameters = new List<SugarParameter>(){
                new SugarParameter("@tableNames", tableNames)
            };

            return Repo.Ado.SqlQuery<GenTable>(sql, parameters);
        }
        #endregion

        #region Private Methods

        // 查询 mysql 表信息
        private ISugarQueryable<GenTable> GetMySqlDbQueryable(GenTableDto dto)
        {
            var sql = $@"
                SELECT table_name, table_comment, create_time, update_time 
                FROM information_schema.tables
		        WHERE table_schema = (SELECT database())
		        AND table_name NOT LIKE 'qrtz_%' AND table_name NOT LIKE 'gen_%'
		        AND table_name NOT IN (SELECT table_name FROM gen_table)
            ";
            var parameters = new List<SugarParameter>();
            if (!string.IsNullOrEmpty(dto.TableName))
            {
                sql += "AND lower(table_name) LIKE lower(concat('%', @tableName, '%'))";
                parameters.Add(new SugarParameter("@tableName", dto.TableName));
            }
            if (!string.IsNullOrEmpty(dto.TableComment))
            {
                sql += "AND lower(table_comment) LIKE lower(concat('%', @tableComment, '%'))";
                parameters.Add(new SugarParameter("@tableComment", dto.TableComment));
            }
            if (dto.Params.BeginTime != null)
            {
                sql += "AND date_format(create_time,'%y%m%d') >= date_format(@BeginTime,'%y%m%d')";
                parameters.Add(new SugarParameter("@BeginTime", dto.Params.BeginTime));
            }
            if (dto.Params.BeginTime != null)
            {
                sql += "AND date_format(create_time,'%y%m%d') <= date_format(@EndTime,'%y%m%d')";
                parameters.Add(new SugarParameter("@EndTime", dto.Params.EndTime));
            }

            return base.SqlQueryable(sql, parameters);
        }

        // 查询 SqlServer 表信息
        private ISugarQueryable<GenTable> GetSqlServerDbQueryable(GenTableDto dto)
        {
            var sql = $@"
                SELECT t.[name] AS [table_name], p.[value] AS [table_comment], t.[crdate] AS [create_time], '' AS [update_time]
                FROM  sys.sysobjects t 
                LEFT JOIN sys.extended_properties p ON t.id = p.major_id AND p.minor_id = 0
                WHERE t.xtype = 'U' AND t.name <> 'dtproperties'
		        AND t.[name] NOT LIKE 'qrtz_%' AND t.[name] NOT LIKE 'gen_%'
		        AND t.[name] NOT IN (SELECT table_name FROM gen_table)
            ";
            var parameters = new List<SugarParameter>();
            if (!string.IsNullOrEmpty(dto.TableName))
            {
                sql += "AND lower(t.[name]) LIKE lower(concat('%', @tableName, '%'))";
                parameters.Add(new SugarParameter("@tableName", dto.TableName));
            }
            if (!string.IsNullOrEmpty(dto.TableComment))
            {
                sql += "AND lower(p.[value]) LIKE lower(concat('%', @tableComment, '%'))";
                parameters.Add(new SugarParameter("@tableComment", dto.TableComment));
            }
            if (dto.Params.BeginTime != null)
            {
                sql += "AND DATEDIFF(DAY, @BeginTime, t.[crdate]) >= 0";
                parameters.Add(new SugarParameter("@BeginTime", dto.Params.BeginTime));
            }
            if (dto.Params.BeginTime != null)
            {
                sql += "AND DATEDIFF(DAY, @EndTime, t.[crdate]) <= 0";
                parameters.Add(new SugarParameter("@EndTime", dto.Params.EndTime));
            }

            return base.SqlQueryable(sql, parameters);
        }

        #endregion
    }
}
