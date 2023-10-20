using SqlSugar;

namespace RuoYi.Data.Dtos
{
    public class TreeDto : BaseDto
    {
        /** 父菜单名称 */
        private string ParentName { get; set; }

        /** 父菜单ID */
        private long ParentId { get; set; }

        /** 显示顺序 */
        private int? OrderNum { get; set; }

        /** 祖级列表 */
        private string Ancestors { get; set; }

        ///** 子部门 */
        //private List<?> Children = new List<>();
    }
}
