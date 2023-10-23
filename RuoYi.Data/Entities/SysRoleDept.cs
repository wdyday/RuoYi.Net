using SqlSugar;

namespace RuoYi.Data.Entities
{
    /// <summary>
    ///  角色和部门关联表 对象 sys_role_dept
    ///  author ruoyi.net
    ///  date   2023-08-23 09:43:52
    /// </summary>
    [SugarTable("sys_role_dept", "角色和部门关联表")]
    public class SysRoleDept : BaseEntity
    {
        /// <summary>
        /// 角色ID (role_id)
        /// </summary>
        [SugarColumn(ColumnName = "role_id", ColumnDescription = "角色ID")]
        public long RoleId { get; set; }
        /// <summary>
        /// 部门ID (dept_id)
        /// </summary>
        [SugarColumn(ColumnName = "dept_id", ColumnDescription = "部门ID")]
        public long DeptId { get; set; }
    }
}
