namespace RuoYi.Framework.RemoteRequest;

/// <summary>
/// HttpDelete 请求
/// </summary>
[SuppressSniffer, AttributeUsage(AttributeTargets.Method)]
public class DeleteAttribute : HttpMethodBaseAttribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public DeleteAttribute() : base(HttpMethod.Delete)
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="requestUrl"></param>
    public DeleteAttribute(string requestUrl) : base(requestUrl, HttpMethod.Delete)
    {
    }
}