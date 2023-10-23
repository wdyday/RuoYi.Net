using RuoYi.Data.Entities;
using SqlSugar;

namespace RuoYi.Quartz.Entities
{
    /// <summary>
    ///  定时任务调度日志表 对象 sys_job_log
    ///  author ruoyi.net
    ///  date   2023-10-12 17:31:30
    /// </summary>
    [SugarTable("sys_job_log", "定时任务调度日志表")]
    public class SysJobLog : BaseEntity
    {
        /// <summary>
        /// 任务日志ID (job_log_id)
        /// </summary>
        [SugarColumn(ColumnName = "job_log_id", ColumnDescription = "任务日志ID", IsPrimaryKey = true, IsIdentity = true)]
        public long JobLogId { get; set; }
                
        /// <summary>
        /// 任务名称 (job_name)
        /// </summary>
        [SugarColumn(ColumnName = "job_name", ColumnDescription = "任务名称")]
        public string JobName { get; set; }
                
        /// <summary>
        /// 任务组名 (job_group)
        /// </summary>
        [SugarColumn(ColumnName = "job_group", ColumnDescription = "任务组名")]
        public string JobGroup { get; set; }
                
        /// <summary>
        /// 调用目标字符串 (invoke_target)
        /// </summary>
        [SugarColumn(ColumnName = "invoke_target", ColumnDescription = "调用目标字符串")]
        public string InvokeTarget { get; set; }
                
        /// <summary>
        /// 日志信息 (job_message)
        /// </summary>
        [SugarColumn(ColumnName = "job_message", ColumnDescription = "日志信息")]
        public string? JobMessage { get; set; }
                
        /// <summary>
        /// 执行状态（0正常 1失败） (status)
        /// </summary>
        [SugarColumn(ColumnName = "status", ColumnDescription = "执行状态（0正常 1失败）")]
        public string? Status { get; set; }
                
        /// <summary>
        /// 异常信息 (exception_info)
        /// </summary>
        [SugarColumn(ColumnName = "exception_info", ColumnDescription = "异常信息")]
        public string? ExceptionInfo { get; set; }
                
    }
}
