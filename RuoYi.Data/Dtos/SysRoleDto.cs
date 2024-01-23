using RuoYi.Data.Attributes;
using System.ComponentModel.DataAnnotations;

namespace RuoYi.Data.Dtos
{
    public class SysRoleDto : BaseDto
    {
        /** 角色ID */
        [Excel(Name = "角色序号")]
        public long RoleId{ get; set; }

        /** 角色名称 */
        [Excel(Name = "角色名称")]
        [Required(ErrorMessage = "角色名称不能为空"), MaxLength(30, ErrorMessage = "角色名称不能超过30个字符")]
        public string? RoleName{ get; set; }

        /** 角色权限(admin/common) */
        [Excel(Name = "角色权限")]
        [Required(ErrorMessage = "角色权限不能为空"), MaxLength(100, ErrorMessage = "角色权限不能超过100个字符")]
        public string? RoleKey{ get; set; }

        /** 角色排序 */
        [Excel(Name = "角色排序")]
        [Required(ErrorMessage = "显示顺序不能为空")]
        public int? RoleSort{ get; set; }

        /** 数据范围（1：所有数据权限；2：自定义数据权限；3：本部门数据权限；4：本部门及以下数据权限；5：仅本人数据权限） */
        public string? DataScope{ get; set; }

        [Excel(Name = "数据范围")]
        public string? DataScopeDesc { get; set; }

        /** 菜单树选择项是否关联显示（ 0：父子不互相关联显示 1：父子互相关联显示） */
        public bool? MenuCheckStrictly{ get; set; }

        /** 部门树选择项是否关联显示（0：父子不互相关联显示 1：父子互相关联显示 ） */
        public bool? DeptCheckStrictly{ get; set; }

        /** 角色状态（0正常 1停用） */
        public string? Status{ get; set; }

        [Excel(Name = "角色状态")]
        public string? StatusDesc { get; set; }

        /** 删除标志（0代表存在 2代表删除） */
        public string? DelFlag{ get; set; }

        /** 用户是否存在此角色标识 默认不存在 */
        public bool? Flag { get; set; } = false;

        /** 菜单组 */
        public long[]? MenuIds { get; set; }

        /** 部门组（数据权限） */
        public long[]? DeptIds { get; set; }

        /** 角色菜单权限 */
        public List<string>? Permissions { get; set; }

        // 用户id
        public long? UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string? UserName { get; set; }

    }
}
