using Microsoft.Extensions.DependencyInjection;
using RazorEngineCore;
using RuoYi.Generator.Options;

namespace RuoYi.Generator;

[AppStartup(300)]
public sealed class Startup : AppStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // 配置热重载
        services.AddConfigurableOptions<GenOptions>();

        // RazorEngine
        services.AddScoped<IRazorEngine, RazorEngine>();
    }
}