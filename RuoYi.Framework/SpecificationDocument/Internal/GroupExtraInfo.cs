namespace RuoYi.Framework.SpecificationDocument;

/// <summary>
/// 分组附加信息
/// </summary>
[SuppressSniffer]
public sealed class GroupExtraInfo
{
    /// <summary>
    /// 分组名
    /// </summary>
    public string Group { get; internal set; }

    /// <summary>
    /// 分组排序
    /// </summary>
    public int Order { get; internal set; }

    /// <summary>
    /// 是否可见
    /// </summary>
    public bool Visible { get; internal set; }
}