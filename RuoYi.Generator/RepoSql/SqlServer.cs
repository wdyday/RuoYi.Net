using RuoYi.Generator.Dtos;

namespace RuoYi.Generator.RepoSql
{
    public class SqlServer : IDbSql, ITransient
    {
        // Table
        public SqlAndParameter GetDbTableListSqlAndParameter(GenTableDto dto)
        {
            var sql = $@"
                SELECT t.[name] AS [table_name], p.[value] AS [table_comment], t.[crdate] AS [create_time], t.[refdate] AS [update_time]
                FROM  sys.sysobjects t 
                LEFT JOIN sys.extended_properties p ON t.id = p.major_id AND p.minor_id = 0
                WHERE t.xtype = 'U' AND t.name <> 'dtproperties'
		        AND t.[name] NOT LIKE 'qrtz_%' AND t.[name] NOT LIKE 'gen_%'
		        AND t.[name] NOT IN (SELECT table_name FROM gen_table)
            ";
            var parameters = new List<SugarParameter>();
            if (!string.IsNullOrEmpty(dto.TableName))
            {
                sql += "AND t.[name] LIKE CONCAT('%', @tableName, '%')";
                parameters.Add(new SugarParameter("@tableName", dto.TableName));
            }
            if (!string.IsNullOrEmpty(dto.TableComment))
            {
                sql += "AND CAST(p.[value] AS VARCHAR(1000)) LIKE CONCAT('%', @tableComment, '%')";
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

            return new SqlAndParameter
            {
                Sql = sql,
                Parameters = parameters
            };
        }

        // Table: 按名字查询
        public string GetDbTableListByNamesSql()
        {
            return $@"
                SELECT t.[name] AS [table_name], p.[value] AS [table_comment], t.[crdate] AS [create_time], t.[refdate] AS [update_time]                
                FROM  sys.sysobjects t 
                LEFT JOIN sys.extended_properties p ON t.id = p.major_id AND p.minor_id = 0
		        WHERE t.xtype = 'U' AND t.name <> 'dtproperties'
		        AND t.[name] NOT LIKE 'qrtz_%' AND t.[name] NOT LIKE 'gen_%'
		        AND t.[name] IN (@tableNames)
            ";
        }

        // 列 按table名称查询
        public string GetDbTableColumnsByName()
        {
            return $@"
            SELECT TableName,
                LTRIM(RTRIM(ColumnName)) AS [column_name],
                [ColumnCNName] AS [column_comment],
                CONCAT([ColumnType], '(',  [MaxLength], ')') AS [column_type],
                -- CASE WHEN [ColumnType] IN ('char','VARCHAR', 'NVARCHAR', 'text', 'xml', 'varbinary', 'image') THEN CONCAT([ColumnType], '(',  [MaxLength], ')') ELSE [ColumnType] END AS [column_type],
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
        }
    }
}
