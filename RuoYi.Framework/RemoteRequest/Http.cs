using RuoYi.Framework.RemoteRequest;

namespace RuoYi.Framework.RemoteRequest;

/// <summary>
/// 远程请求静态类
/// </summary>
[SuppressSniffer]
public static class Http
{
    /// <summary>
    /// 获取远程请求代理
    /// </summary>
    /// <typeparam name="THttpDispatchProxy">远程请求代理对象</typeparam>
    /// <returns><see cref="GetHttpProxy{THttpDispatchProxy}"/></returns>
    public static THttpDispatchProxy GetHttpProxy<THttpDispatchProxy>()
        where THttpDispatchProxy : class, IHttpDispatchProxy
    {
        return App.GetService<THttpDispatchProxy>(App.RootServices);
    }
}