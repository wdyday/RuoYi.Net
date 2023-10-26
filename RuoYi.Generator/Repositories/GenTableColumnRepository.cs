namespace RuoYi.Generator.Repositories
{
    public class GenTableColumnRepository : BaseRepository<GenTableColumn, GenTableColumnDto>
    {
        public GenTableColumnRepository(ISqlSugarRepository<GenTableColumn> sqlSugarRepository)
        {
            Repo = sqlSugarRepository;
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

        //public ISugarQueryable<GenTableColumn> DbQueryable(long tableId)
        //{
        //    var sql = $@"
        //        select column_id, table_id, column_name, column_comment, column_type, java_type, java_field, is_pk, is_increment, is_required, is_insert, is_edit, is_list, is_query, query_type, html_type, dict_type, sort, create_by, create_time, update_by, update_time 
        //        from gen_table_column
        //        where table_id = @tableId
        //        order by sort
        //    ";
        //    var parameters = new List<SugarParameter>(){
        //        new SugarParameter("@tableId",$"{tableId}")
        //    };

        //    return base.SqlQueryable(sql, parameters);
        //}

        public List<GenTableColumn> SelectDbTableColumnsByName(string tableName)
        {
            var dbType = Repo.Context.CurrentConnectionConfig.DbType;
            var quaryable = dbType switch
            {
                DbType.MySql => GetMySqlDbQueryable(tableName),
                DbType.SqlServer => GetSqlServerDbQueryable(tableName),
                //DbType.Oracle => GetMySqlDbQueryable(tableName),
                _ => GetMySqlDbQueryable(tableName),
            };

            return quaryable.ToList();
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

        #region Private Methods

        // 查询 mysql 表信息
        private ISugarQueryable<GenTableColumn> GetMySqlDbQueryable(string tableName)
        {
            var sql = $@"
                SELECT column_name, (case when (is_nullable = 'no' && column_key != 'PRI') then '1' else '0' end) as is_required, (case when column_key = 'PRI' then '1' else '0' end) as is_pk, ordinal_position as sort, column_comment, (case when extra = 'auto_increment' then '1' else '0' end) as is_increment, column_type
		        FROM information_schema.columns WHERE table_schema = (SELECT database()) and table_name = (@tableName)
		        ORDER BY ordinal_position
            ";
            var parameters = new List<SugarParameter>(){
                new SugarParameter("@tableName",$"{tableName}")
            };

            return base.SqlQueryable(sql, parameters);
        }

        // 查询 SqlServer 表信息
        private ISugarQueryable<GenTableColumn> GetSqlServerDbQueryable(string tableName)
        {
            var sql = $@"
                SELECT TableName,
                LTRIM(RTRIM(ColumnName)) AS [column_name],
                [ColumnCNName] AS [column_comment],
                CASE WHEN [ColumnType] IN ('char','VARCHAR', 'NVARCHAR', 'text', 'xml', 'varbinary', 'image') THEN CONCAT([ColumnType], '(',  [MaxLength], ')') ELSE [ColumnType] END AS [column_type],
	            [NetType] AS [net_type],
                [MaxLength] AS [max_length],
                IsKey AS [is_pk],
                CASE WHEN t.[IsNull] = 0 THEN 1 ELSE 0 END AS [is_required],
                t.[IsIdentity] AS [is_increment],
	            t.[Sort] AS [sort]
            FROM (
	            SELECT obj.name AS [TableName] ,
                    col.name AS [ColumnName] ,
                    CONVERT(NVARCHAR(100),ISNULL(ep.[value], '')) AS [ColumnCNName],
                    t.name AS [ColumnType],		
		            CASE WHEN t.name = 'uniqueidentifier' THEN 'guid'
			            WHEN t.name IN('smallint', 'INT') THEN 'int'
			            WHEN t.name = 'bigint' THEN 'long'
			            WHEN t.name IN('char','VARCHAR', 'NVARCHAR', 'text', 'xml', 'varbinary', 'image') THEN 'string'
			            WHEN t.name IN('tinyint') THEN 'byte'
			            WHEN t.name IN('bit') THEN 'bool'
			            WHEN t.name IN('time', 'date', 'DATETIME', 'smallDATETIME') THEN 'DateTime'
			            WHEN t.name IN('smallmoney', 'DECIMAL', 'numeric', 'money') THEN 'decimal'
			            WHEN t.name = 'float' THEN 'float'
			            ELSE 'string'
		            END AS [NetType],
                    CASE WHEN col.length < 1 THEN 0 WHEN col.name IN ('NVARCHAR','NCHAR') THEN col.length/2 ELSE col.length END AS [MaxLength],
                    CASE WHEN EXISTS (SELECT  1
                                        FROM dbo.sysindexes si
                                        INNER JOIN dbo.sysindexkeys sik ON si.id = sik.id AND si.indid = sik.indid
                                        INNER JOIN dbo.syscolumns sc ON sc.id = sik.id AND sc.colid = sik.colid
                                        INNER JOIN dbo.sysobjects so ON so.name = si.name AND so.xtype = 'PK'
                                        WHERE sc.id = col.id AND sc.colid = col.colid
		            ) THEN 1 ELSE 0 END AS [IsKey],
                    CASE WHEN col.isnullable = 1 THEN 1 ELSE 0 END AS[IsNull],
                    columnproperty(col.id, col.name, 'IsIdentity') [IsIdentity],
                    col.colorder AS [Sort]
                FROM  dbo.syscolumns col
                LEFT JOIN dbo.systypes t ON col.xtype = t.xusertype
                INNER JOIN dbo.sysobjects obj ON col.id = obj.id AND obj.xtype IN ( 'U','V')
                LEFT JOIN dbo.syscomments comm ON col.cdefault = comm.id
                LEFT JOIN sys.extended_properties ep ON col.id = ep.major_id AND col.colid = ep.minor_id AND ep.name = 'MS_Description'
                LEFT JOIN sys.extended_properties epTwo ON obj.id = epTwo.major_id AND epTwo.minor_id = 0 AND epTwo.name = 'MS_Description'
                WHERE obj.name = @tableName
            ) AS t
            ORDER BY t.[Sort]
            ";
            var parameters = new List<SugarParameter>(){
                new SugarParameter("@tableName",$"{tableName}")
            };

            return base.SqlQueryable(sql, parameters);
        }
        #endregion
    }
}
