using SqlSugar;

namespace RuoYi.Data.Entities
{
    /// <summary>
    ///  系统访问记录 对象 sys_logininfor
    ///  author ruoyi
    ///  date   2023-10-07 16:52:43
    /// </summary>
    [SugarTable("sys_logininfor", "系统访问记录")]
    public class SysLogininfor : BaseEntity
    {
        /// <summary>
        /// 访问ID (info_id)
        /// </summary>
        [SugarColumn(ColumnName = "info_id", ColumnDescription = "访问ID", IsPrimaryKey = true, IsIdentity = true)]
        public long InfoId { get; set; }
                
        /// <summary>
        /// 用户账号 (user_name)
        /// </summary>
        [SugarColumn(ColumnName = "user_name", ColumnDescription = "用户账号")]
        public string? UserName { get; set; }
                
        /// <summary>
        /// 登录IP地址 (ipaddr)
        /// </summary>
        [SugarColumn(ColumnName = "ipaddr", ColumnDescription = "登录IP地址")]
        public string? Ipaddr { get; set; }
                
        /// <summary>
        /// 登录地点 (login_location)
        /// </summary>
        [SugarColumn(ColumnName = "login_location", ColumnDescription = "登录地点")]
        public string? LoginLocation { get; set; }
                
        /// <summary>
        /// 浏览器类型 (browser)
        /// </summary>
        [SugarColumn(ColumnName = "browser", ColumnDescription = "浏览器类型")]
        public string? Browser { get; set; }
                
        /// <summary>
        /// 操作系统 (os)
        /// </summary>
        [SugarColumn(ColumnName = "os", ColumnDescription = "操作系统")]
        public string? Os { get; set; }
                
        /// <summary>
        /// 登录状态（0成功 1失败） (status)
        /// </summary>
        [SugarColumn(ColumnName = "status", ColumnDescription = "登录状态（0成功 1失败）")]
        public string? Status { get; set; }
                
        /// <summary>
        /// 提示消息 (msg)
        /// </summary>
        [SugarColumn(ColumnName = "msg", ColumnDescription = "提示消息")]
        public string? Msg { get; set; }
                
        /// <summary>
        /// 访问时间 (login_time)
        /// </summary>
        [SugarColumn(ColumnName = "login_time", ColumnDescription = "访问时间")]
        public DateTime? LoginTime { get; set; }
                
    }
}
