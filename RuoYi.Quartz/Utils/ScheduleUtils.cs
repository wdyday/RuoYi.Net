using Quartz;
using Quartz.Impl;
using RuoYi.Framework.Exceptions;
using RuoYi.Quartz.Constants;
using RuoYi.Quartz.Enums;
using RuoYi.Quartz.Jobs;
using System.Collections.Specialized;

namespace RuoYi.Quartz.Utils
{
    public static class ScheduleUtils
    {
        private static ISchedulerFactory _schedulerFactory;

        private static ISchedulerFactory CreateSchedulerFactory()
        {
            if (_schedulerFactory != null)
                return _schedulerFactory;

            NameValueCollection properties = new NameValueCollection
            {
                [StdSchedulerFactory.PropertySchedulerName] = "RuoYi.Quartz.Scheduler",
                //[StdSchedulerFactory.PropertySchedulerInstanceName] = "QuartzScheduler"
            };
            return new StdSchedulerFactory(properties);
        }

        public static async Task<IScheduler> GetDefaultScheduleAsync()
        {
            _schedulerFactory = CreateSchedulerFactory();
            return await _schedulerFactory.GetScheduler();
        }

        /// <summary>
        /// 构建任务触发对象
        /// </summary>
        public static TriggerKey GetTriggerKey(long jobId, string jobGroup)
        {
            return new TriggerKey(ScheduleConstants.TASK_CLASS_NAME + jobId, jobGroup);
        }

        /// <summary>
        /// 构建任务键对象
        /// </summary>
        public static JobKey GetJobKey(long jobId, string jobGroup)
        {
            return JobKey.Create(ScheduleConstants.TASK_CLASS_NAME + jobId, jobGroup);
        }

        /// <summary>
        /// 创建定时任务
        /// </summary>
        public static async Task CreateScheduleJob(SysJobDto job)
        {
            var scheduler = await GetDefaultScheduleAsync();
            await CreateScheduleJob(scheduler, job);
        }

        /// <summary>
        /// 创建定时任务
        /// </summary>
        public static async Task CreateScheduleJob(IScheduler scheduler, SysJobDto job)
        {
            var jobType = GetQuartzJobType(job);

            // 构建job信息
            long jobId = job.JobId;
            string jobGroup = job.JobGroup!;
            var jobKey = GetJobKey(jobId, jobGroup);
            IJobDetail jobDetail = JobBuilder.Create(jobType).WithIdentity(jobKey).Build();

            // 表达式调度构建器
            CronScheduleBuilder cronScheduleBuilder = CronScheduleBuilder.CronSchedule(job.CronExpression!);
            // 按新的cronExpression表达式构建一个新的trigger
            var trigger = TriggerBuilder.Create()
                .ForJob(jobKey)
                .WithIdentity(GetTriggerKey(jobId, jobGroup))
                .WithCronSchedule(job.CronExpression!, builder =>
                {
                    HandleCronScheduleMisfirePolicy(job, cronScheduleBuilder);
                }).Build();

            // 放入参数，运行时的方法可以获取
            jobDetail.JobDataMap.Add(ScheduleConstants.TASK_PROPERTIES, job);

            // 判断是否存在
            if (await scheduler.CheckExists(jobKey))
            {
                // 防止创建时存在数据问题 先移除，然后在执行创建操作
                await scheduler.DeleteJob(jobKey);
            }

            // 判断任务是否过期
            if (CronUtils.GetNextExecution(job!.CronExpression!) != null)
            {
                // 执行调度任务
                await scheduler.ScheduleJob(jobDetail, trigger);
            }

            // 暂停任务
            if (job.Status!.Equals(ScheduleStatus.PAUSE.GetValue()))
            {
                await scheduler.PauseJob(jobKey);
            }
        }

        /// <summary>
        /// 检查包名是否为白名单配置
        /// job白名单配置: JobWhiteListAssembly, Assembly 集合, 多个逗号分隔
        /// </summary>
        /// <param name="invokeTarget">目标字符串</param>
        /// <returns></returns>
        public static bool InWhiteList(string invokeTarget)
        {
            var whiteListAssembly = App.GetConfig<string>("JobWhiteListAssembly");
            var whiteListAssemblies = !string.IsNullOrEmpty(whiteListAssembly) ? whiteListAssembly.Split(",") : new string[] { };
            var jobWhiteList = whiteListAssemblies.Length > 0 ? whiteListAssemblies : ScheduleConstants.JOB_WHITELIST_STR;

            string packageName = StringUtils.SubstringBefore(invokeTarget, "(") ?? "";
            int count = StringUtils.CountMatches(packageName, ".");
            if (count > 1)
            {
                return StringUtils.ContainsAnyIgnoreCase(invokeTarget, jobWhiteList);
            }
            string classNamespace = AssemblyUtils.GetTaskAttributeClassNamespace(invokeTarget.Split(".")[0]);
            return StringUtils.ContainsAnyIgnoreCase(classNamespace, jobWhiteList)
                    && !StringUtils.ContainsAnyIgnoreCase(classNamespace, ScheduleConstants.JOB_ERROR_STR);
        }

        //public static QuartzScheduler CreateQuartzScheduler(string name, string instanceId, int threadCount)
        //{
        //    var threadPool = new DefaultThreadPool { MaxConcurrency = threadCount };
        //    threadPool.Initialize();

        //    QuartzSchedulerResources res = new QuartzSchedulerResources
        //    {
        //        Name = name,
        //        InstanceId = instanceId,
        //        ThreadPool = threadPool,
        //        JobRunShellFactory = new StdJobRunShellFactory(),
        //        JobStore = new RAMJobStore(),
        //        //IdleWaitTime = TimeSpan.FromMilliseconds(10),
        //        MaxBatchSize = threadCount,
        //        BatchTimeWindow = TimeSpan.FromMilliseconds(10)
        //    };

        //    var scheduler = new QuartzScheduler(res, TimeSpan.FromMilliseconds(10));
        //    scheduler.JobFactory = new SimpleJobFactory();
        //    return scheduler;
        //}

        #region private methods

        /// <summary>
        /// 得到quartz任务类
        /// </summary>
        /// <param name="sysJob">执行计划</param>
        /// <returns>具体执行任务类</returns>
        private static Type GetQuartzJobType(SysJobDto sysJob)
        {
            bool isConcurrent = "0".Equals(sysJob.Concurrent);
            return isConcurrent ? typeof(QuartzExecution) : typeof(QuartzDisallowConcurrentExecution);
        }

        // 设置定时任务策略
        private static void HandleCronScheduleMisfirePolicy(SysJobDto job, CronScheduleBuilder cb)
        {
            switch (job.MisfirePolicy)
            {
                case ScheduleConstants.MISFIRE_DEFAULT:
                    return;
                case ScheduleConstants.MISFIRE_IGNORE_MISFIRES:
                    cb.WithMisfireHandlingInstructionIgnoreMisfires();
                    return;
                case ScheduleConstants.MISFIRE_FIRE_AND_PROCEED:
                    cb.WithMisfireHandlingInstructionFireAndProceed();
                    return;
                case ScheduleConstants.MISFIRE_DO_NOTHING:
                    cb.WithMisfireHandlingInstructionDoNothing();
                    return;
                default:
                    throw new ServiceException("The task misfire policy '" + job.MisfirePolicy
                            + "' cannot be used in cron schedule tasks", TaskCode.CONFIG_ERROR.ToInt());
            }
        }
        #endregion
    }
}
