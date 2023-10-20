using SqlSugar;

namespace RuoYi.Data.Entities
{
    [SugarTable("sys_user", "用户表")]
    public class SysUser : UserBaseEntity
    {
        /** 用户ID */
        [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID", IsPrimaryKey = true, IsIdentity = true)]
        public long UserId { get; set; }

        /** 部门ID */
        [SugarColumn(ColumnName = "dept_id", ColumnDescription = "部门编号")]
        public long DeptId { get; set; }

        /** 用户账号 */
        [SugarColumn(ColumnName = "user_name", ColumnDescription = "登录名称")]
        public string? UserName { get; set; }

        /** 用户昵称 */
        [SugarColumn(ColumnName = "nick_name", ColumnDescription = "用户名称")]
        public string? NickName { get; set; }

        /** 用户邮箱 */
        [SugarColumn(ColumnName = "email", ColumnDescription = "用户邮箱")]
        public string? Email { get; set; }

        /** 手机号码 */
        [SugarColumn(ColumnName = "phonenumber", ColumnDescription = "手机号码")]
        public string? Phonenumber { get; set; }

        /** 用户性别 */
        [SugarColumn(ColumnName = "sex", ColumnDescription = "用户性别")]
        public string? Sex { get; set; }

        /** 用户头像 */
        [SugarColumn(ColumnName = "avatar", ColumnDescription = "用户头像")]
        public string? Avatar { get; set; }

        /** 密码 */
        [SugarColumn(ColumnName = "password", ColumnDescription = "密码")]
        public string? Password { get; set; }

        /** 帐号状态（0正常 1停用） */
        [SugarColumn(ColumnName = "status", ColumnDescription = "帐号状态")]
        public string? Status { get; set; }

        /** 删除标志（0代表存在 2代表删除） */
        [SugarColumn(ColumnName = "del_flag", ColumnDescription = "删除标志")]
        public string? DelFlag { get; set; }

        /** 最后登录IP */
        [SugarColumn(ColumnName = "login_ip", ColumnDescription = "最后登录IP")]
        public string? LoginIp { get; set; }

        /** 最后登录时间 */
        [SugarColumn(ColumnName = "login_date", ColumnDescription = "最后登录时间")]
        public DateTime? LoginDate { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnName = "remark", ColumnDescription = "备注")]
        public string? Remark { get; set; }

        /** 部门对象 */
        //[SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToOne, nameof(DeptId))]
        public SysDept? Dept { get; set; }

        ///** 角色对象 */
        //[SugarColumn(IsIgnore = true)]
        //public List<SysRole> Roles { get; set; }

        ///** 角色组 */
        //[SugarColumn(IsIgnore = true)]
        //public List<long> RoleIds { get; set; }

        ///** 岗位组 */
        //[SugarColumn(IsIgnore = true)]
        //public List<long> PostIds { get; set; }

        ///** 角色ID */
        //[SugarColumn(IsIgnore = true)]
        //public long RoleId { get; set; }

    }
}
