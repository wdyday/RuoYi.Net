using System.Collections.Generic;
namespace RuoYi.Data.Dtos
{
    /// <summary>
    ///  角色和部门关联表 对象 sys_role_dept
    ///  author ruoyi.net
    ///  date   2023-08-23 09:43:52
    /// </summary>
    public class SysRoleDeptDto : BaseDto
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public long? RoleId { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public long? DeptId { get; set; }
    }
}
