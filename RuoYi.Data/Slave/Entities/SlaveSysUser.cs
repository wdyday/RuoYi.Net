using RuoYi.Data.Entities;
using SqlSugar;

namespace RuoYi.Data.Slave.Entities
{
    [Tenant(DataConstants.Slave)]
    [SugarTable("sys_user", "用户表")]
    public class SlaveSysUser : UserBaseEntity
    {
        /** 用户ID */
        //@Excel(name = "用户序号", cellType = ColumnType.NUMERIC, prompt = "用户编号")
        [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID")]
        public long UserId { get; set; }

        /** 部门ID */
        //@Excel(name = "部门编号", type = Type.IMPORT)
        [SugarColumn(ColumnName = "dept_id", ColumnDescription = "部门编号")]
        public long DeptId { get; set; }

        /** 用户账号 */
        //@Excel(name = "登录名称")
        [SugarColumn(ColumnName = "user_name", ColumnDescription = "登录名称")]
        public string? UserName { get; set; }

        /** 用户昵称 */
        //@Excel(name = "用户名称")
        [SugarColumn(ColumnName = "nick_name", ColumnDescription = "用户名称")]
        public string? NickName { get; set; }

        /** 用户邮箱 */
        //@Excel(name = "用户邮箱")
        [SugarColumn(ColumnName = "email", ColumnDescription = "用户邮箱")]
        public string? Email { get; set; }

        /** 手机号码 */
        //@Excel(name = "手机号码")
        [SugarColumn(ColumnName = "phonenumber", ColumnDescription = "手机号码")]
        public string? PhoneNumber { get; set; }

        /** 用户性别 */
        //@Excel(name = "用户性别", readConverterExp = "0=男,1=女,2=未知")
        [SugarColumn(ColumnName = "sex", ColumnDescription = "用户性别")]
        public string? Sex { get; set; }

        /** 用户头像 */
        [SugarColumn(ColumnName = "avatar", ColumnDescription = "用户头像")]
        public string? Avatar { get; set; }

        /** 密码 */
        [SugarColumn(ColumnName = "password", ColumnDescription = "密码")]
        public string? Password { get; set; }

        /** 帐号状态（0正常 1停用） */
        //@Excel(name = "帐号状态", readConverterExp = "0=正常,1=停用")
        [SugarColumn(ColumnName = "status", ColumnDescription = "帐号状态")]
        public string? Status { get; set; }

        /** 删除标志（0代表存在 2代表删除） */
        [SugarColumn(ColumnName = "del_flag", ColumnDescription = "删除标志")]
        public string? DelFlag { get; set; }

        /** 最后登录IP */
        //@Excel(name = "最后登录IP", type = Type.EXPORT)
        [SugarColumn(ColumnName = "login_ip", ColumnDescription = "最后登录IP")]
        public string? LoginIp { get; set; }

        /** 最后登录时间 */
        //@Excel(name = "最后登录时间", width = 30, dateFormat = "yyyy-MM-dd HH:mm:ss", type = Type.EXPORT)
        [SugarColumn(ColumnName = "login_date", ColumnDescription = "最后登录时间")]
        public DateTime LoginDate { get; set; }

        /** 部门对象 */
        //@Excels({
        //    @Excel(name = "部门名称", targetAttr = "deptName", type = Type.EXPORT),
        //@Excel(name = "部门负责人", targetAttr = "leader", type = Type.EXPORT)
        //})
        [SugarColumn(IsIgnore = true)]
        public SysDept Dept { get; set; }

        /** 角色对象 */
        [SugarColumn(IsIgnore = true)]
        public List<SysRole> Roles { get; set; }

        /** 角色组 */
        [SugarColumn(IsIgnore = true)]
        public long[] RoleIds { get; set; }

        /** 岗位组 */
        [SugarColumn(IsIgnore = true)]
        public long[] PostIds { get; set; }

        /** 角色ID */
        [SugarColumn(IsIgnore = true)]
        public long RoleId { get; set; }

    }
}
