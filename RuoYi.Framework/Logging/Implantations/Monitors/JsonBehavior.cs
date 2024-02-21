using System.ComponentModel;

namespace RuoYi.Framework.Logging;

/// <summary>
/// LoggingMonitor JSON 输出行为
/// </summary>
[SuppressSniffer]
public enum JsonBehavior
{
    /// <summary>
    /// 不输出 JSON 格式
    /// </summary>
    /// <remarks>默认值，输出文本日志</remarks>
    [Description("不输出 JSON 格式")]
    None = 0,

    /// <summary>
    /// 只输出 JSON 格式
    /// </summary>
    [Description("只输出 JSON 格式")]
    OnlyJson = 1,

    /// <summary>
    /// 输出 JSON 格式和文本日志
    /// </summary>
    [Description("输出 JSON 格式和文本日志")]
    All = 2
}