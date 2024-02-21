namespace RuoYi.Framework.RemoteRequest;

/// <summary>
/// 远程请求失败事件类
/// </summary>
[SuppressSniffer]
public sealed class HttpRequestFaildedEventArgs : EventArgs
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="request"></param>
    /// <param name="response"></param>
    /// <param name="exception"></param>
    public HttpRequestFaildedEventArgs(HttpRequestMessage request, HttpResponseMessage response, Exception exception)
    {
        Request = request;
        Response = response;
        Exception = exception;
    }

    /// <summary>
    /// 请求对象
    /// </summary>
    public HttpRequestMessage Request { get; internal set; }

    /// <summary>
    /// 响应对象
    /// </summary>
    public HttpResponseMessage Response { get; internal set; }

    /// <summary>
    /// 异常对象
    /// </summary>
    public Exception Exception { get; internal set; }
}