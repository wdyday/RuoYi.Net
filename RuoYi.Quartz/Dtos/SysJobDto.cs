using RuoYi.Data.Attributes;

namespace RuoYi.Quartz.Dtos
{
    /// <summary>
    ///  定时任务调度表 对象 sys_job
    ///  author ruoyi.net
    ///  date   2023-10-12 17:31:30
    /// </summary>
    public class SysJobDto : BaseDto
    {
        /// <summary>
        /// 任务ID
        /// </summary>
        [Excel(Name = "任务序号")]
        public long JobId { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        [Excel(Name = "任务名称")]
        [Required(ErrorMessage = "任务名称不能为空"), MaxLength(64, ErrorMessage = "任务名称不能超过64个字符")]
        public string? JobName { get; set; }

        /// <summary>
        /// 任务组名
        /// </summary>
        [Excel(Name = "任务组名")]
        public string? JobGroup { get; set; }

        /// <summary>
        /// 调用目标字符串
        /// </summary>
        [Excel(Name = "调用目标字符串")]
        [Required(ErrorMessage = "调用目标字符串不能为空"), MaxLength(500, ErrorMessage = "调用目标字符串长度不能超过500个字符")]
        public string? InvokeTarget { get; set; }

        /// <summary>
        /// cron执行表达式
        /// </summary>
        [Excel(Name = "执行表达式")]
        [Required(ErrorMessage = "Cron执行表达式不能为空"), MaxLength(255, ErrorMessage = "Cron执行表达式不能超过255个字符")]
        public string? CronExpression { get; set; }

        /// <summary>
        /// 计划执行错误策略（0=默认,1=立即触发执行,2=触发一次执行,3=不触发立即执行）
        /// </summary>
        public string? MisfirePolicy { get; set; }
        [Excel(Name = "计划策略")]
        public string? MisfirePolicyDesc { get; set; }

        /// <summary>
        /// 是否并发执行（0允许 1禁止）
        /// </summary>
        public string? Concurrent { get; set; }
        [Excel(Name = "并发执行")]
        public string? ConcurrentDesc { get; set; }

        /// <summary>
        /// 状态（0正常 1暂停）
        /// </summary>
        public string? Status { get; set; }
        [Excel(Name = "任务状态")]
        public string? StatusDesc { get; set; }

    }
}
