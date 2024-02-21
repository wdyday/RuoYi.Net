using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace RuoYi.Framework;

/// <summary>
/// 模拟 Startup，解决 .NET5 下不设置 UseStartup 时出现异常问题
/// </summary>
[SuppressSniffer]
public sealed class FakeStartup
{
    /// <summary>
    /// 配置服务
    /// </summary>
    public void ConfigureServices(IServiceCollection _)
    {
    }

    /// <summary>
    /// 配置请求
    /// </summary>
    public void Configure(IApplicationBuilder _)
    {
    }
}