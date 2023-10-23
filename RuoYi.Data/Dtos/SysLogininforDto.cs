namespace RuoYi.Data.Dtos
{
    /// <summary>
    ///  系统访问记录 对象 sys_logininfor
    ///  author ruoyi
    ///  date   2023-08-22 10:07:36
    /// </summary>
    public class SysLogininforDto : BaseDto
    {
        /// <summary>
        /// 访问ID
        /// </summary>
        public long InfoId { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// 登录IP地址
        /// </summary>
        public string? Ipaddr { get; set; }

        /// <summary>
        /// 登录地点
        /// </summary>
        public string? LoginLocation { get; set; }

        /// <summary>
        /// 浏览器类型
        /// </summary>
        public string? Browser { get; set; }

        /// <summary>
        /// 操作系统
        /// </summary>
        public string? Os { get; set; }

        /// <summary>
        /// 登录状态（0成功 1失败）
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// 提示消息
        /// </summary>
        public string? Msg { get; set; }

        /// <summary>
        /// 访问时间
        /// </summary>
        public DateTime? LoginTime { get; set; }
    }
}
