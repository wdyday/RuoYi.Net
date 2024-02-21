namespace RuoYi.Framework.RemoteRequest;

/// <summary>
/// HttpPut 请求
/// </summary>
[SuppressSniffer, AttributeUsage(AttributeTargets.Method)]
public class PutAttribute : HttpMethodBaseAttribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public PutAttribute() : base(HttpMethod.Put)
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="requestUrl"></param>
    public PutAttribute(string requestUrl) : base(requestUrl, HttpMethod.Put)
    {
    }
}