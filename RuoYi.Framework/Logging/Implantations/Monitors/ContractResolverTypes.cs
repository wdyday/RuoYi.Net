namespace RuoYi.Framework.Logging;

/// <summary>
/// LoggingMonitor 序列化属性命名规则选项
/// </summary>
[SuppressSniffer]
public enum ContractResolverTypes
{
    /// <summary>
    /// CamelCase 小驼峰
    /// </summary>
    /// <remarks>默认值</remarks>
    CamelCase = 0,

    /// <summary>
    /// 保持原样
    /// </summary>
    Default = 1
}