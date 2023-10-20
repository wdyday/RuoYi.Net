using Microsoft.Extensions.DependencyInjection;
using UAParser.Extensions;

namespace RuoYi.System;

[AppStartup(210)]
public sealed class Startup : AppStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // User agent service
        services.AddUserAgentParser();
    }
}