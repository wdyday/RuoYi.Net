using Quartz;
using RuoYi.Quartz.Dtos;
using RuoYi.Quartz.Entities;
using RuoYi.Quartz.Utils;

namespace RuoYi.Quartz.Jobs
{
    [DisallowConcurrentExecution]
    public class QuartzDisallowConcurrentExecution : AbstractQuartzJob
    {
        protected override void DoExecute(IJobExecutionContext context, SysJobDto sysJob)
        {
            JobInvokeUtils.InvokeMethod(sysJob);
        }
    }
}
