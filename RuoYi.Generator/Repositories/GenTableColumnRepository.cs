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

        public ISugarQueryable<GenTableColumn> DbQueryable(long tableId)
        {
            var sql = $@"
                select column_id, table_id, column_name, column_comment, column_type, java_type, java_field, is_pk, is_increment, is_required, is_insert, is_edit, is_list, is_query, query_type, html_type, dict_type, sort, create_by, create_time, update_by, update_time 
                from gen_table_column
                where table_id = @tableId
                order by sort
            ";
            var parameters = new List<SugarParameter>(){
                new SugarParameter("@tableId",$"{tableId}")
            };

            return base.SqlQueryable(sql, parameters);
        }

        public List<GenTableColumn> SelectDbTableColumnsByName(string tableName)
        {
            var sql = $@"
                SELECT column_name, (case when (is_nullable = 'no' && column_key != 'PRI') then '1' else '0' end) as is_required, (case when column_key = 'PRI' then '1' else '0' end) as is_pk, ordinal_position as sort, column_comment, (case when extra = 'auto_increment' then '1' else '0' end) as is_increment, column_type
		        FROM information_schema.columns WHERE table_schema = (SELECT database()) and table_name = (@tableName)
		        ORDER BY ordinal_position
            ";
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
}
