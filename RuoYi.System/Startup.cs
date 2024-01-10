using Microsoft.Extensions.DependencyInjection;
using RuoYi.System.Services;
using UAParser.Extensions;

namespace RuoYi.System;

[AppStartup(210)]
public sealed class Startup : AppStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // User agent service
        services.AddUserAgentParser();

        // 缓存初始化
        LoadingConfigCache();
    }

    private void LoadingConfigCache()
    {
        try
        {
            var sysConfigService = App.GetService<SysConfigService>();
            sysConfigService.LoadingConfigCache();
        }
        catch (Exception ex)
        {
            Log.Error("LoadingConfigCache error: {}", ex, ex.Message);
        }
    }
}