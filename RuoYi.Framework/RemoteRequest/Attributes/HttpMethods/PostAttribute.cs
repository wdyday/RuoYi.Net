namespace RuoYi.Framework.RemoteRequest;

/// <summary>
/// HttpPost 请求
/// </summary>
[SuppressSniffer, AttributeUsage(AttributeTargets.Method)]
public class PostAttribute : HttpMethodBaseAttribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public PostAttribute() : base(HttpMethod.Post)
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="requestUrl"></param>
    public PostAttribute(string requestUrl) : base(requestUrl, HttpMethod.Post)
    {
    }
}