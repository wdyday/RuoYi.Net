using System.ComponentModel;

namespace RuoYi.Quartz.Enums
{
    /// <summary>
    /// 计划执行错误策略（0=默认,1=触发立即执行,2=触发一次执行,3=不触发立即执行）
    /// </summary>
    public enum JobMisfirePolicy
    {
        [Description("默认")]
        Default,
        [Description("触发立即执行")]
        TriggerExecuteNow,
        [Description("触发一次执行")]
        TriggerExecuteOnce,
        [Description("不触发立即执行")]
        NotTriggerExecuteNow
    }
}
