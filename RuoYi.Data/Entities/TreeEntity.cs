using SqlSugar;

namespace RuoYi.Data.Entities
{
    public class TreeEntity : UserBaseEntity
    {
        /** 父菜单名称 */
        [SugarColumn(ColumnName = "parent_name", ColumnDescription = "父菜单名称")]
        private string ParentName { get; set; }

        /** 父菜单ID */
        [SugarColumn(ColumnName = "parent_id", ColumnDescription = "父菜单ID")]
        private long ParentId { get; set; }

        /** 显示顺序 */
        [SugarColumn(ColumnName = "order_num", ColumnDescription = "显示顺序")]
        private int? OrderNum { get; set; }

        /** 祖级列表 */
        [SugarColumn(ColumnName = "ancestors", ColumnDescription = "祖级列表")]
        private string Ancestors { get; set; }

        ///** 子部门 */
        //private List<?> Children = new List<>();
    }
}
