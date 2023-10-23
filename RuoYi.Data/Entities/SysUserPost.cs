using SqlSugar;

namespace RuoYi.Data.Entities
{
    /// <summary>
    ///  用户与岗位关联表 对象 sys_user_post
    ///  author ruoyi.net
    ///  date   2023-08-23 09:43:50
    /// </summary>
    [SugarTable("sys_user_post", "用户与岗位关联表")]
    public class SysUserPost : BaseEntity
    {
        /// <summary>
        /// 用户ID (user_id)
        /// </summary>
        [SugarColumn(ColumnName = "user_id", ColumnDescription = "用户ID")]
        public long UserId { get; set; }
        /// <summary>
        /// 岗位ID (post_id)
        /// </summary>
        [SugarColumn(ColumnName = "post_id", ColumnDescription = "岗位ID")]
        public long PostId { get; set; }
    }
}
