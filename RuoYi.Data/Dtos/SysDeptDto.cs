using System.ComponentModel.DataAnnotations;

namespace RuoYi.Data.Dtos
{
    public class SysDeptDto : BaseDto
    {
        /** 部门ID */
        public long? DeptId{ get; set; }

        /** 父部门ID */
        public long? ParentId{ get; set; }

        /** 祖级列表 */
        public string? Ancestors{ get; set; }

        /** 部门名称 */
        [Required(ErrorMessage = "部门名称不能为空"), MaxLength(30, ErrorMessage = "部门名称不能超过30个字符")]
        public string? DeptName{ get; set; }

        /** 显示顺序 */
        [Required(ErrorMessage = "显示顺序不能为空")]
        public int? OrderNum{ get; set; }

        /** 负责人 */
        public string? Leader{ get; set; }

        /** 联系电话 */
        [MaxLength(30, ErrorMessage = "联系电话长度不能超过11个字符")]
        public string? Phone{ get; set; }

        /** 邮箱 */
        [EmailAddress(ErrorMessage = "邮箱格式不正确"), MaxLength(50, ErrorMessage = "邮箱长度不能超过50个字符")]
        public string? Email{ get; set; }

        /** 部门状态:0正常,1停用 */
        public string? Status{ get; set; }

        /** 删除标志（0代表存在 2代表删除） */
        public string? DelFlag{ get; set; }

        /** 父部门名称 */
        public string? ParentName{ get; set; }

        /** 子部门 */
        public List<SysDeptDto>? Children { get; set; }

        /** 部门树选择项是否关联显示（0：父子不互相关联显示 1：父子互相关联显示 ） */
        public bool? DeptCheckStrictly { get; set; }
        // 角色ID
        public long? RoleId { get; set; }


        /** 父部门ID */
        public List<long>? ParentIds { get; set; }
    }
}
