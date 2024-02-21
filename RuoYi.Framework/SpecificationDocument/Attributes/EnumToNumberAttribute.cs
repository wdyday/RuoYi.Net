namespace RuoYi.Framework.SpecificationDocument;

/// <summary>
/// 用于控制 Swager 生成 Enum 类型
/// </summary>
[SuppressSniffer, AttributeUsage(AttributeTargets.Enum, AllowMultiple = false)]
public sealed class EnumToNumberAttribute : Attribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public EnumToNumberAttribute()
        : this(true)
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="enabled">启用状态</param>
    public EnumToNumberAttribute(bool enabled = true)
    {
        Enabled = enabled;
    }

    /// <summary>
    /// 启用状态
    /// </summary>
    /// <remarks>设置 false 则使用字符串类型</remarks>
    public bool Enabled { get; set; }
}