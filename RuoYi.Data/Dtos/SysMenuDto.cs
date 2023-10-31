using System.ComponentModel.DataAnnotations;

namespace RuoYi.Data.Dtos
{
    public class SysMenuDto : BaseDto
    {
        /** 菜单ID */
        public long MenuId { get; set; }

        /** 菜单名称 */
        [Required(ErrorMessage = "菜单名称不能为空"), MaxLength(50, ErrorMessage = "菜单名称长度不能超过50个字符")]
        public string? MenuName{ get; set; }

        /** 父菜单名称 */
        public string? ParentName { get; set; }

        /** 父菜单ID */
        public long ParentId { get; set; }

        /** 显示顺序 */
        [Required(ErrorMessage = "显示顺序不能为空")]
        public int? OrderNum{ get; set; }

        /** 路由地址 */
        [MaxLength(200, ErrorMessage = "路由地址不能超过200个字符")]
        public string? Path{ get; set; }

        /** 组件路径 */
        [MaxLength(255, ErrorMessage = "组件路径不能超过255个字符")]
        public string? Component{ get; set; }

        /** 路由参数 */
        public string? Query{ get; set; }

        /** 是否为外链（0是 1否） */
        public string? IsFrame{ get; set; }

        /** 是否缓存（0缓存 1不缓存） */
        public string? IsCache{ get; set; }

        /** 类型（M目录 C菜单 F按钮） */
        [Required(ErrorMessage = "菜单类型不能为空")]
        public string? MenuType{ get; set; }

        /** 显示状态（0显示 1隐藏） */
        public string? Visible{ get; set; }

        /** 菜单状态（0正常 1停用） */
        public string? Status{ get; set; }

        /** 权限字符串 */
        [MaxLength(100, ErrorMessage = "权限标识长度不能超过100个字符")]
        public string? Perms{ get; set; }

        /** 菜单图标 */
        public string? Icon{ get; set; }

        /** 子菜单 */
        public List<SysMenuDto> Children = new List<SysMenuDto>();

        /// <summary>
        /// 角色状态
        /// </summary>
        public string? RoleStatus { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public long RoleId { get; set; }


        /** 类型（M目录 C菜单 F按钮） */
        public List<string> MenuTypes { get; set; } = new List<string>();

    }
}
