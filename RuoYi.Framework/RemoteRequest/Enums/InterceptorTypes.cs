using System.ComponentModel;

namespace RuoYi.Framework.RemoteRequest;

/// <summary>
/// 拦截类型
/// </summary>
[SuppressSniffer]
public enum InterceptorTypes
{
    /// <summary>
    /// 创建 HttpClient 拦截
    /// </summary>
    [Description("创建 HttpClient 拦截")]
    Initiate,

    /// <summary>
    /// HttpClient 拦截
    /// </summary>
    [Description("HttpClient 拦截")]
    Client,

    /// <summary>
    /// HttpRequestMessage 拦截
    /// </summary>
    [Description("HttpRequestMessage 拦截")]
    Request,

    /// <summary>
    /// HttpResponseMessage 拦截
    /// </summary>
    [Description("HttpResponseMessage 拦截")]
    Response,

    /// <summary>
    /// 异常拦截
    /// </summary>
    [Description("异常拦截")]
    Exception
}