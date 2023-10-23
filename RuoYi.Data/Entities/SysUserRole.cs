using SqlSugar;

namespace RuoYi.Data.Entities
{
    /// <summary>
    ///  用户和角色关联表 对象 sys_user_role
    ///  author ruoyi.net
    ///  date   2023-08-23 09:43:52
    /// </summary>
    [SugarTable("sys_user_role", "用户和角色关联表")]
    public class SysUserRole: BaseEntity
    {
        /// <summary>
        /// 用户ID (user_id)
        /// </summary>
        [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID")]
        public long UserId { get; set; }
        /// <summary>
        /// 角色ID (role_id)
        /// </summary>
        [SugarColumn(ColumnName = "role_id", ColumnDescription = "角色ID")]
        public long RoleId { get; set; }
    }
}
