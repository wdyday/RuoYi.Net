using RuoYi.Data.Dtos;

namespace RuoYi.Quartz.Dtos
{
    /// <summary>
    ///  定时任务调度日志表 对象 sys_job_log
    ///  author ruoyi.net
    ///  date   2023-10-12 17:31:30
    /// </summary>
    public class SysJobLogDto : BaseDto
    {
        /// <summary>
        /// 任务日志ID
        /// </summary>
        public long JobLogId { get; set; }
                
        /// <summary>
        /// 任务名称
        /// </summary>
        public string JobName { get; set; }
                
        /// <summary>
        /// 任务组名
        /// </summary>
        public string JobGroup { get; set; }
                
        /// <summary>
        /// 调用目标字符串
        /// </summary>
        public string InvokeTarget { get; set; }
                
        /// <summary>
        /// 日志信息
        /// </summary>
        public string? JobMessage { get; set; }
                
        /// <summary>
        /// 执行状态（0正常 1失败）
        /// </summary>
        public string? Status { get; set; }
                
        /// <summary>
        /// 异常信息
        /// </summary>
        public string? ExceptionInfo { get; set; }
                
    }
}
