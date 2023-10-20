using SqlSugar;

namespace RuoYi.Data.Entities
{
    [SugarTable("sys_dept", "部门表")]
    public class SysDept : UserBaseEntity
    {
        /** 部门ID */
        [SugarColumn(ColumnName = "dept_id", ColumnDescription = "部门ID", IsPrimaryKey = true, IsIdentity = true)]
        public long DeptId{ get; set; }

        /** 父部门ID */
        [SugarColumn(ColumnName = "parent_id", ColumnDescription = "父部门ID")]
        public long ParentId{ get; set; }

        /** 祖级列表 */
        [SugarColumn(ColumnName = "ancestors", ColumnDescription = "祖级列表")]
        public string? Ancestors{ get; set; }

        /** 部门名称 */
        [SugarColumn(ColumnName = "dept_name", ColumnDescription = "部门名称")]
        public string? DeptName{ get; set; }

        /** 显示顺序 */
        [SugarColumn(ColumnName = "order_num", ColumnDescription = "显示顺序")]
        public int? OrderNum{ get; set; }

        /** 负责人 */
        [SugarColumn(ColumnName = "leader", ColumnDescription = "负责人")]
        public string? Leader{ get; set; }

        /** 联系电话 */
        [SugarColumn(ColumnName = "phone", ColumnDescription = "联系电话")]
        public string? Phone{ get; set; }

        /** 邮箱 */
        [SugarColumn(ColumnName = "email", ColumnDescription = "邮箱")]
        public string? Email{ get; set; }

        /** 部门状态:0正常,1停用 */
        [SugarColumn(ColumnName = "status", ColumnDescription = "部门状态:0正常,1停用")]
        public string? Status{ get; set; }

        /** 删除标志（0代表存在 2代表删除） */
        [SugarColumn(ColumnName = "del_flag", ColumnDescription = "删除标志（0代表存在 2代表删除）")]
        public string? DelFlag{ get; set; }

        /** 父部门名称 */
        [SugarColumn(IsIgnore = true)]
        public string? ParentName{ get; set; }

        /** 子部门 */
        [SugarColumn(IsIgnore = true)]
        public List<SysDept> Children { get; set; } = new List<SysDept>();
    }
}
