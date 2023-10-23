using SqlSugar;

namespace RuoYi.Data.Entities
{
    /// <summary>
    ///  角色和菜单关联表 对象 sys_role_menu
    ///  author ruoyi.net
    ///  date   2023-08-23 09:43:53
    /// </summary>
    [SugarTable("sys_role_menu", "角色和菜单关联表")]
    public class SysRoleMenu : BaseEntity
    {
        /// <summary>
        /// 角色ID (role_id)
        /// </summary>
        [SugarColumn(ColumnName = "role_id", ColumnDescription = "角色ID")]
        public long RoleId { get; set; }
        /// <summary>
        /// 菜单ID (menu_id)
        /// </summary>
        [SugarColumn(ColumnName = "menu_id", ColumnDescription = "菜单ID")]
        public long MenuId { get; set; }
    }
}
