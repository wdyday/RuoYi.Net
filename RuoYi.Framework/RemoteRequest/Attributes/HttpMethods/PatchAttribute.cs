namespace RuoYi.Framework.RemoteRequest;

/// <summary>
/// HttpPatch 请求
/// </summary>
[SuppressSniffer, AttributeUsage(AttributeTargets.Method)]
public class PatchAttribute : HttpMethodBaseAttribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public PatchAttribute() : base(HttpMethod.Patch)
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="requestUrl"></param>
    public PatchAttribute(string requestUrl) : base(requestUrl, HttpMethod.Patch)
    {
    }
}