using Newtonsoft.Json;
using RuoYi.Data.Attributes;
using System.ComponentModel.DataAnnotations;

namespace RuoYi.Data.Dtos
{
    public class SysUserDto : BaseDto
    {
        /** 用户ID */
        [Excel(Name = "用户序号")]
        public long? UserId { get; set; }

        /** 部门ID */
        [Excel(Name = "部门编号", Type = ExcelOperationType.Import)]
        public long? DeptId { get; set; }

        /** 用户账号 */
        [Excel(Name = "登录名称")]
        [Required(ErrorMessage = "用户账号不能为空"), MaxLength(30, ErrorMessage = "用户账号长度不能超过30个字符")]
        public string? UserName { get; set; }

        /** 用户昵称 */
        [Excel(Name = "用户名称")]
        [StringLength(30, ErrorMessage = "用户昵称长度不能超过30个字符")]
        public string? NickName { get; set; }

        /** 用户邮箱 */
        [Excel(Name = "用户邮箱")]
        [EmailAddress(ErrorMessage = "邮箱格式不正确"), MaxLength(50, ErrorMessage = "邮箱长度不能超过50个字符")]
        public string? Email { get; set; }

        /** 手机号码 */
        [Excel(Name = "手机号码")]
        [StringLength(11, ErrorMessage = "手机号码长度不能超过11个字符")]
        public string? Phonenumber { get; set; }

        /** 用户性别 0=男,1=女,2=未知 */
        public string? Sex { get; set; }

        [Excel(Name = "用户性别")]
        public string? SexDesc { get; set; }

        /** 用户头像 */
        public string? Avatar { get; set; }

        /** 密码 */
        //[Newtonsoft.Json.JsonIgnore]
        public string? Password { get; set; }

        /** 帐号状态（0正常 1停用） */
        public string? Status { get; set; }

        [Excel(Name = "帐号状态")]
        public string? StatusDesc { get; set; }

        /** 删除标志（0代表存在 2代表删除） */
        public string? DelFlag { get; set; }

        /** 最后登录IP */
        [Excel(Name = "最后登录IP", Type = ExcelOperationType.Export)]
        public string? LoginIp { get; set; }

        /** 最后登录时间 */
        [Excel(Name = "最后登录时间", Format = "yyyy-MM-dd HH:mm:ss", Type = ExcelOperationType.Export)]
        public DateTime LoginDate { get; set; }

        /** 部门对象 */
        public SysDeptDto? Dept { get; set; }

        [Excel(Name = "部门名称", Type = ExcelOperationType.Export)]
        public string? DeptName { get; set; }

        [Excel(Name = "部门负责人", Type = ExcelOperationType.Export)]
        public string? DeptLeader { get; set; }

        /** 角色对象 */
        public List<SysRoleDto>? Roles { get; set; }

        /** 角色组 */
        public List<long>? RoleIds { get; set; }

        /** 岗位组 */
        public List<long>? PostIds { get; set; }

        /** 角色ID */
        public long? RoleId { get; set; }

        /// <summary>
        /// 是否 已分配用户角色
        /// </summary>
        public bool? IsAllocated { get; set; }

        /// <summary>
        /// 不序列化 Password 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializePassword()
        {
            return false;
        }
    }
}
