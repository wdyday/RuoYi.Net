using RuoYi.Framework;

namespace Microsoft.Extensions.Hosting;

/// <summary>
/// 监听泛型主机启动事件
/// </summary>
internal class GenericHostLifetimeEventsHostedService : IHostedService
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="host"></param>
    public GenericHostLifetimeEventsHostedService(IHost host)
    {
        // 存储根服务
        InternalApp.RootServices = host.Services;
    }

    /// <summary>
    /// 监听主机启动
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// 监听主机停止
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}