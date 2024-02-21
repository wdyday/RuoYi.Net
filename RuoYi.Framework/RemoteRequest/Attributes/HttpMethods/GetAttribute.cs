namespace RuoYi.Framework.RemoteRequest;

/// <summary>
/// HttpGet 请求
/// </summary>
[SuppressSniffer, AttributeUsage(AttributeTargets.Method)]
public class GetAttribute : HttpMethodBaseAttribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public GetAttribute() : base(HttpMethod.Get)
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="requestUrl"></param>
    public GetAttribute(string requestUrl) : base(requestUrl, HttpMethod.Get)
    {
    }
}