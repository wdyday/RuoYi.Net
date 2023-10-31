using RuoYi.Generator.Dtos;

namespace RuoYi.Generator.RepoSql
{
    public class MySql : IDbSql, ITransient
    {
        // Table 查询
        public SqlAndParameter GetDbTableListSqlAndParameter(GenTableDto dto)
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
                SELECT table_name, table_comment, create_time, update_time 
                FROM information_schema.tables
		        WHERE table_schema = (SELECT database())
		        AND table_name NOT LIKE 'qrtz_%' AND table_name NOT LIKE 'gen_%'
		        AND table_name IN (@tableNames)
            ";
        }

        // 列 按table名称查询
        public string GetDbTableColumnsByName()
        {
            return $@"
                SELECT column_name, (case when (is_nullable = 'no' && column_key != 'PRI') then '1' else '0' end) as is_required, (case when column_key = 'PRI' then '1' else '0' end) as is_pk, ordinal_position as sort, column_comment, (case when extra = 'auto_increment' then '1' else '0' end) as is_increment, column_type
		        FROM information_schema.columns WHERE table_schema = (SELECT database()) and table_name = (@tableName)
		        ORDER BY ordinal_position
            ";
        }
    }
}
