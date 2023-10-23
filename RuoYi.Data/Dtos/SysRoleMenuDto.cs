using System.Collections.Generic;
namespace RuoYi.Data.Dtos
{
    /// <summary>
    ///  角色和菜单关联表 对象 sys_role_menu
    ///  author ruoyi.net
    ///  date   2023-08-23 09:43:53
    /// </summary>
    public class SysRoleMenuDto : BaseDto
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public long? RoleId { get; set; }
        /// <summary>
        /// 菜单ID
        /// </summary>
        public long? MenuId { get; set; }
    }
}
