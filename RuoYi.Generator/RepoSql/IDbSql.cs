using RuoYi.Generator.Dtos;

namespace RuoYi.Generator.RepoSql
{
    public interface IDbSql
    {
        // Table 查询
        SqlAndParameter GetDbTableListSqlAndParameter(GenTableDto dto);

        // Table: 按名字查询
        string GetDbTableListByNamesSql();

        // 列 按table名称查询
        string GetDbTableColumnsByName();
    }
}
