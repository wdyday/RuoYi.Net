using System.ComponentModel;

namespace RuoYi.Quartz.Enums
{
    /// <summary>
    /// 是否并发执行（0允许 1禁止)
    /// </summary>
    public enum ConcurrentStatus
    {
        [Description("允许")]
        ALLOWED,
        [Description("禁止")]
        FORBIDDEN
    }
}
