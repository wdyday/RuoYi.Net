namespace RuoYi.Framework.Options;

/// <summary>
/// 选项校验失败消息特性
/// </summary>
[SuppressSniffer, AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class FailureMessageAttribute : Attribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="text">文本内容</param>
    public FailureMessageAttribute(string text)
    {
        Text = text;
    }

    /// <summary>
    /// 文本内容
    /// </summary>
    public string Text { get; set; }
}