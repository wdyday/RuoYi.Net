using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(RuoYi.Framework.HostingStartup))]

namespace RuoYi.Framework;

/// <summary>
/// 配置程序启动时自动注入
/// </summary>
[SuppressSniffer]
public sealed class HostingStartup : IHostingStartup
{
    /// <summary>
    /// 配置应用启动
    /// </summary>
    /// <param name="builder"></param>
    public void Configure(IWebHostBuilder builder)
    {
        InternalApp.ConfigureApplication(builder);
    }
}