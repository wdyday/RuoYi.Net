using RuoYi.Data.Dtos;

namespace RuoYi.Data.Slave.Dtos
{
    public class SlaveSysUserDto : BaseDto
    {
        /** 用户ID */
        public long UserId { get; set; }

        /** 部门ID */
        public long DeptId { get; set; }

        /** 用户账号 */
        public string? UserName { get; set; }

        /** 用户昵称 */
        public string? NickName { get; set; }

        /** 用户邮箱 */
        public string? Email { get; set; }

        /** 手机号码 */
        public string? PhoneNumber { get; set; }

        /** 用户性别 */
        public string? Sex { get; set; }

        /** 用户头像 */
        public string? Avatar { get; set; }

        /** 密码 */
        public string? Password { get; set; }

        /** 帐号状态（0正常 1停用） */
        public string? Status { get; set; }

        /** 删除标志（0代表存在 2代表删除） */
        public string? DelFlag { get; set; }

        /** 最后登录IP */
        public string? LoginIp { get; set; }

        /** 最后登录时间 */
        public DateTime LoginDate { get; set; }

        /** 部门对象 */
        public SysDeptDto Dept { get; set; }

        /** 角色对象 */
        public List<SysRoleDto> Roles { get; set; }

        /** 角色组 */
        public long[] RoleIds { get; set; }

        /** 岗位组 */
        public long[] PostIds { get; set; }

        /** 角色ID */
        public long RoleId { get; set; }


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
