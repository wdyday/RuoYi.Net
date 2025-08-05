using RuoYi.Framework.Attributes;
using RuoYi.Quartz.Dtos;
using RuoYi.Quartz.Services;

namespace RuoYi.Admin.Tasks
{
    [Task("testTask")]
    public class TestTask
    {
        private readonly SysJobService _sysJobService;

        public TestTask()
        {
            _sysJobService = App.GetService<SysJobService>();
        }


        public void getAllJobs()
        {
            _sysJobService.GetDtoList(new SysJobDto()).ForEach(e =>
            {
                RuoYi.Framework.Logging.Log.Information($"任务ID：{e.JobId}，任务名称：{e.JobName}，任务组名：{e.JobGroup}，cron表达式：{e.CronExpression}");
            });

        }
    }
}
