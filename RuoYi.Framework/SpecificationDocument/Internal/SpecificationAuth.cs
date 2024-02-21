namespace RuoYi.Framework.SpecificationDocument;

/// <summary>
/// 规范化文档授权参数类型
/// </summary>
[SuppressSniffer]
public sealed class SpecificationAuth
{
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }
}