using RuoYi.Generator.Dtos;

namespace RuoYi.Generator.RepoSql
{
    public class RepoSqlService: ITransient
    {
        private readonly ISqlSugarRepository _sqlSugarRepository;
        private readonly MySql _mySql;
        private readonly SqlServer _sqlServer;
        public RepoSqlService(ISqlSugarRepository sqlSugarRepository, MySql mySql, SqlServer sqlServer)
        {
            _sqlSugarRepository = sqlSugarRepository;
            _mySql = mySql;
            _sqlServer = sqlServer;
        }

        public DbType GetDbType()
        {
            return _sqlSugarRepository.Context.CurrentConnectionConfig.DbType;
        }

        public IDbSql GetDbSql()
        {
            var dbType = this.GetDbType();
            return dbType switch
            {
                DbType.MySql => _mySql,
                DbType.SqlServer => _sqlServer,
                //DbType.Oracle => _oracle,
                _ => _mySql
            };
        }

        // Table 查询
        public SqlAndParameter GetDbTableListSqlAndParameter(GenTableDto dto)
        {
            return this.GetDbSql().GetDbTableListSqlAndParameter(dto);
        }

        // Table: 按名字查询
        public string GetDbTableListByNamesSql()
        {
            return this.GetDbSql().GetDbTableListByNamesSql();
        }

        // 列 按table名称查询
        public string GetDbTableColumnsByName()
        {
            return this.GetDbSql().GetDbTableColumnsByName();
        }
    }
}
