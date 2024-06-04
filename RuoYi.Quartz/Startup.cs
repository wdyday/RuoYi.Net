using Microsoft.Extensions.DependencyInjection;
using RuoYi.Quartz.Services;

namespace RuoYi.Quartz;

[AppStartup(100)]
public sealed class Startup : AppStartup
{
    private static Thread _scheduleThread = new Thread(InitThreadSchedule);

    public void ConfigureServices(IServiceCollection services)
    {
        _scheduleThread.Start();
    }

    private static void InitThreadSchedule()
    {
        Task.Run(async () =>
        {
            await Task.Delay(10000);

            var jobService = App.GetService<SysJobService>();
            await jobService.InitSchedule();
        });
    }
}