namespace RuoYi.Framework.Options;

/// <summary>
/// 选项构建器特性
/// </summary>
[SuppressSniffer, AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class OptionsBuilderAttribute : Attribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public OptionsBuilderAttribute()
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="sectionKey">配置节点</param>
    public OptionsBuilderAttribute(string sectionKey)
    {
        SectionKey = sectionKey;
    }

    /// <summary>
    /// 配置节点
    /// </summary>
    public string SectionKey { get; set; }

    /// <summary>
    /// 未知配置节点抛异常
    /// </summary>
    public bool ErrorOnUnknownConfiguration { get; set; }

    /// <summary>
    /// 绑定非公开属性
    /// </summary>
    public bool BindNonPublicProperties { get; set; }

    /// <summary>
    /// 启用验证特性支持
    /// </summary>
    public bool ValidateDataAnnotations { get; set; }

    /// <summary>
    /// 验证选项类型
    /// </summary>
    public Type[] ValidateOptionsTypes { get; set; }
}