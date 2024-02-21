using Quartz;
using RuoYi.Framework.Logging;
using RuoYi.Quartz.Constants;
using RuoYi.Quartz.Dtos;
using RuoYi.Quartz.Entities;
using RuoYi.Quartz.Services;

namespace RuoYi.Quartz.Jobs
{
    public abstract class AbstractQuartzJob : IJob
    {
        private static long _startTimestamp;

        public async Task Execute(IJobExecutionContext context)
        {
            SysJobDto? sysJob = context.MergedJobDataMap[ScheduleConstants.TASK_PROPERTIES] as SysJobDto;
            if (sysJob == null)
            {
                return;
            }

            try
            {
                Before(context, sysJob);
                if (sysJob != null)
                {
                    DoExecute(context, sysJob);
                }
                await After(context, sysJob!, null);
            }
            catch (Exception e)
            {
                Log.Error("任务执行异常  - ", e);
                await After(context, sysJob, e);
            }
        }

        private void Before(IJobExecutionContext context, SysJobDto sysJob)
        {
            Interlocked.Exchange(ref _startTimestamp, DateTime.Now.ToUnixTimeMilliseconds());
        }

        private async Task After(IJobExecutionContext context, SysJobDto sysJob, Exception? e)
        {
            var startTimestamp = Interlocked.Read(ref _startTimestamp);
            var now = DateTime.Now;
            long runMs = now.ToUnixTimeMilliseconds() - startTimestamp;
            string? errorMsg = e != null ? StringUtils.Substring(ExceptionUtils.GetErrorMessage(e), 0, 2000) : null;

            SysJobLog sysJobLog = new SysJobLog
            {
                JobName = sysJob.JobName,
                JobGroup = sysJob.JobGroup,
                InvokeTarget = sysJob.InvokeTarget,
                JobMessage = $"{sysJob.JobName} 总共耗时: {runMs}毫秒",
                Status = e != null ? Data.Constants.FAIL: Data.Constants.SUCCESS,
                ExceptionInfo = errorMsg
            };

            // 写入数据库当中
            var sysJobLogService = App.GetService<SysJobLogService>();
            await sysJobLogService.InsertAsync(sysJobLog);
        }

        protected abstract void DoExecute(IJobExecutionContext context, SysJobDto sysJob);
    }
}
