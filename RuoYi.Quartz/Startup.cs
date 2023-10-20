using Microsoft.Extensions.DependencyInjection;
using RuoYi.Quartz.Services;

namespace RuoYi.Quartz;

[AppStartup(100)]
public sealed class Startup : AppStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        InitSchedule();
    }

    private void InitSchedule()
    {
        var jobService = App.GetService<SysJobService>();
        jobService.InitSchedule().GetAwaiter().GetResult();
    }
}