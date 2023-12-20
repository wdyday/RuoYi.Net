using RuoYi.Data.Entities;
using SqlSugar;

namespace RuoYi.Quartz.Entities
{
    /// <summary>
    ///  定时任务调度表 对象 sys_job
    ///  author ruoyi.net
    ///  date   2023-10-12 17:31:30
    /// </summary>
    [SugarTable("sys_job", "定时任务调度表")]
    public class SysJob : BaseEntity
    {
        /// <summary>
        /// 任务ID (job_id)
        /// </summary>
        [SugarColumn(ColumnName = "job_id", ColumnDescription = "任务ID", IsPrimaryKey = true, IsIdentity = true)]
        public long JobId { get; set; }
                
        /// <summary>
        /// 任务名称 (job_name)
        /// </summary>
        [SugarColumn(ColumnName = "job_name", ColumnDescription = "任务名称", IsPrimaryKey = true)]
        public string JobName { get; set; }
                
        /// <summary>
        /// 任务组名 (job_group)
        /// </summary>
        [SugarColumn(ColumnName = "job_group", ColumnDescription = "任务组名", IsPrimaryKey = true)]
        public string JobGroup { get; set; }
                
        /// <summary>
        /// 调用目标字符串 (invoke_target)
        /// </summary>
        [SugarColumn(ColumnName = "invoke_target", ColumnDescription = "调用目标字符串")]
        public string InvokeTarget { get; set; }
                
        /// <summary>
        /// cron执行表达式 (cron_expression)
        /// </summary>
        [SugarColumn(ColumnName = "cron_expression", ColumnDescription = "cron执行表达式")]
        public string? CronExpression { get; set; }
                
        /// <summary>
        /// 计划执行错误策略（1立即执行 2执行一次 3放弃执行） (misfire_policy)
        /// </summary>
        [SugarColumn(ColumnName = "misfire_policy", ColumnDescription = "计划执行错误策略（1立即执行 2执行一次 3放弃执行）")]
        public string? MisfirePolicy { get; set; }
                
        /// <summary>
        /// 是否并发执行（0允许 1禁止） (concurrent)
        /// </summary>
        [SugarColumn(ColumnName = "concurrent", ColumnDescription = "是否并发执行（0允许 1禁止）")]
        public string? Concurrent { get; set; }
                
        /// <summary>
        /// 状态（0正常 1暂停） (status)
        /// </summary>
        [SugarColumn(ColumnName = "status", ColumnDescription = "状态（0正常 1暂停）")]
        public string? Status { get; set; }
                
    }
}
