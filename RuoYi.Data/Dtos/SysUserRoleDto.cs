namespace RuoYi.Data.Dtos
{
    /// <summary>
    ///  用户和角色关联表 对象 sys_user_role
    ///  author ruoyi.net
    ///  date   2023-08-23 09:43:52
    /// </summary>
    public class SysUserRoleDto : BaseDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }
        public List<long> UserIds { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public long RoleId { get; set; }
    }
}
