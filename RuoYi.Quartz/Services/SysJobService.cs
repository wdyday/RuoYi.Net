using Quartz;
using RuoYi.Framework.Interceptors;
using RuoYi.Quartz.Constants;
using RuoYi.Quartz.Enums;
using RuoYi.Quartz.Repositories;
using RuoYi.Quartz.Utils;

namespace RuoYi.Quartz.Services;

/// <summary>
///  定时任务调度表 Service
///  author ruoyi.net
///  date   2023-10-12 17:38:29
/// </summary>
public class SysJobService : BaseService<SysJob, SysJobDto>, ITransient
{
    private readonly ILogger<SysJobService> _logger;
    private readonly SysJobRepository _sysJobRepository;

    public SysJobService(ILogger<SysJobService> logger,
        SysJobRepository sysJobRepository)
    {
        BaseRepo = sysJobRepository;

        _logger = logger;
        _sysJobRepository = sysJobRepository;
    }

    // 项目启动时，初始化定时器 主要是防止手动修改数据库导致未同步到定时任务处理（注：不能手动修改数据库ID和任务组名，否则会导致脏数据）
    public async Task InitSchedule()
    {
        IScheduler _scheduler = await ScheduleUtils.GetDefaultScheduleAsync();
        await _scheduler.Clear();

        var jobs = _sysJobRepository.GetDtoList(new SysJobDto());
        foreach (var job in jobs)
        {
            await ScheduleUtils.CreateScheduleJob(job);
        }

        await _scheduler.Start();
    }

    /// <summary>
    /// 查询 定时任务调度表 详情
    /// </summary>
    public async Task<SysJob> GetAsync(long id)
    {
        var entity = await base.FirstOrDefaultAsync(e => e.JobId == id);
        return entity;
    }

    /// <summary>
    /// 查询 定时任务调度表 详情
    /// </summary>
    public async Task<SysJobDto> GetDtoAsync(long id)
    {
        var entity = await base.FirstOrDefaultAsync(e => e.JobId == id);
        var dto = entity.Adapt<SysJobDto>();
        return dto;
    }

    /// <summary>
    /// 新增任务
    /// </summary>
    public async Task<bool> InsertJobAsync(SysJobDto job)
    {
        job.Status = ScheduleStatus.PAUSE.GetValue();
        bool success = await _sysJobRepository.InsertAsync(job);
        if (success)
        {
            await ScheduleUtils.CreateScheduleJob(job);
        }
        return success;
    }

    /// <summary>
    /// 更新任务的时间表达式
    /// </summary>
    public async Task<bool> UpdateJobAsync(SysJobDto job)
    {
        SysJob properties = await GetAsync(job.JobId);
        var rows = await _sysJobRepository.UpdateAsync(job);
        if (rows > 0)
        {
            await UpdateSchedulerJob(job, properties.JobGroup);
        }
        return rows > 0;
    }

    /// <summary>
    /// 更新任务
    /// </summary>
    /// <param name="job">任务对象</param>
    /// <param name="jobGroup">任务组名</param>
    public async Task UpdateSchedulerJob(SysJobDto job, string jobGroup)
    {
        IScheduler _scheduler = await ScheduleUtils.GetDefaultScheduleAsync();

        long jobId = job.JobId;
        // 判断是否存在
        JobKey jobKey = ScheduleUtils.GetJobKey(jobId, jobGroup);
        if (await _scheduler.CheckExists(jobKey))
        {
            // 防止创建时存在数据问题 先移除，然后在执行创建操作
            await _scheduler.DeleteJob(jobKey);
        }
        await ScheduleUtils.CreateScheduleJob(_scheduler, job);
    }

    /// <summary>
    /// 验证 job
    /// </summary>
    public string CheckJob(SysJobDto job)
    {
        if (!CronUtils.IsValid(job.CronExpression))
        {
            return "'" + job.JobName + "'失败，Cron表达式不正确";
        }
        else if (StringUtils.ContainsIgnoreCase(job.InvokeTarget, Data.Constants.LOOKUP_RMI))
        {
            return "'" + job.JobName + "'失败，目标字符串不允许'rmi'调用";
        }
        else if (StringUtils.ContainsAnyIgnoreCase(job.InvokeTarget, new string[] { Data.Constants.LOOKUP_LDAP, Data.Constants.LOOKUP_LDAPS }))
        {
            return "'" + job.JobName + "'失败，目标字符串不允许'ldap(s)'调用";
        }
        else if (StringUtils.ContainsAnyIgnoreCase(job.InvokeTarget, new string[] { Data.Constants.HTTP, Data.Constants.HTTPS }))
        {
            return "'" + job.JobName + "'失败，目标字符串不允许'http(s)'调用";
        }
        else if (StringUtils.ContainsAnyIgnoreCase(job.InvokeTarget, Data.Constants.JOB_ERROR_STR))
        {
            return "'" + job.JobName + "'失败，目标字符串存在违规";
        }
        else if (!ScheduleUtils.InWhiteList(job.InvokeTarget!))
        {
            return "'" + job.JobName + "'失败，目标字符串不在白名单内";
        }

        return string.Empty;
    }

    /// <summary>
    /// 任务调度状态修改
    /// </summary>
    [Transactional]
    public virtual async Task<bool> ChangeStatusAsync(SysJobDto job)
    {
        int rows = 0;
        string status = job.Status!;

        var dbJob = await GetAsync(job.JobId);
        if (ScheduleStatus.NORMAL.GetValue().Equals(status))
        {
            rows = await ResumeJob(dbJob);
        }
        else if (ScheduleStatus.PAUSE.GetValue().Equals(status))
        {
            rows = await PauseJob(dbJob);
        }
        return rows > 0;
    }

    /// <summary>
    /// 立即运行任务
    /// </summary>
    /// <param name="job">调度信息</param>
    public async Task<bool> Run(SysJobDto job)
    {
        IScheduler _scheduler = await ScheduleUtils.GetDefaultScheduleAsync();

        bool result = false;
        long jobId = job.JobId;
        string jobGroup = job.JobGroup!;
        SysJobDto properties = await GetDtoAsync(jobId);
        // 参数
        JobDataMap dataMap = new JobDataMap();
        dataMap.Add(ScheduleConstants.TASK_PROPERTIES, properties);
        JobKey jobKey = ScheduleUtils.GetJobKey(jobId, jobGroup);
        if (await _scheduler.CheckExists(jobKey))
        {
            result = true;
            await _scheduler.TriggerJob(jobKey, dataMap);
        }
        return result;
    }

    /// <summary>
    /// 批量删除调度信息
    /// </summary>
    /// <param name="jobIds">需要删除的任务ID</param>
    /// <returns></returns>
    public async Task DeleteJobByIdsAsync(List<long> jobIds)
    {
        foreach (long jobId in jobIds)
        {
            SysJob job = await GetAsync(jobId);
            await DeleteJob(job);
        }
    }

    #region private mothods
    // 暂停任务
    private async Task<int> PauseJob(SysJob job)
    {
        IScheduler _scheduler = await ScheduleUtils.GetDefaultScheduleAsync();

        job.Status = ScheduleStatus.PAUSE.GetValue();
        int rows = await _sysJobRepository.UpdateAsync(job, true);
        if (rows > 0)
        {
            await _scheduler.PauseJob(ScheduleUtils.GetJobKey(job.JobId, job.JobGroup));
        }
        return rows;
    }

    // 恢复任务
    private async Task<int> ResumeJob(SysJob job)
    {
        IScheduler _scheduler = await ScheduleUtils.GetDefaultScheduleAsync();

        job.Status = ScheduleStatus.NORMAL.GetValue();
        int rows = await _sysJobRepository.UpdateAsync(job, true);
        if (rows > 0)
        {
            await _scheduler.ResumeJob(ScheduleUtils.GetJobKey(job.JobId, job.JobGroup));
        }
        return rows;
    }

    // 删除任务
    private async Task<int> DeleteJob(SysJob job)
    {
        IScheduler _scheduler = await ScheduleUtils.GetDefaultScheduleAsync();

        int rows = await _sysJobRepository.DeleteAsync(job.JobId);
        if (rows > 0)
        {
            await _scheduler.DeleteJob(ScheduleUtils.GetJobKey(job.JobId, job.JobGroup));
        }
        return rows;
    }
    #endregion
}