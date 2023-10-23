using Quartz;
using RuoYi.Framework.Exceptions;

namespace RuoYi.Quartz.Utils
{
    public static class CronUtils
    {
        /// <summary>
        /// 返回一个布尔值代表一个给定的Cron表达式的有效性
        /// </summary>
        /// <param name="cronExpression">Cron表达式</param>
        /// <returns>表达式是否有效</returns>
        public static bool IsValid(string? cronExpression)
        {
            if (string.IsNullOrEmpty(cronExpression)) return false;
            return CronExpression.IsValidExpression(cronExpression);
        }

        /// <summary>
        /// 返回一个字符串值,表示该消息无效Cron表达式给出有效性
        /// </summary>
        /// <param name="cronExpression">Cron表达式</param>
        /// <returns>String 无效时返回表达式错误描述,如果有效返回null</returns>
        public static string? GetInvalidMessage(string cronExpression)
        {
            try
            {
                new CronExpression(cronExpression);
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// 返回下一个执行时间根据给定的Cron表达式
        /// </summary>
        /// <param name="cronExpression">Cron表达式</param>
        /// <returns>Date 下次Cron表达式执行时间</returns>
        public static DateTime? GetNextExecution(string cronExpression)
        {
            try
            {
                CronExpression cron = new CronExpression(cronExpression);
                var dateTimeOffset = cron.GetNextValidTimeAfter(DateTimeOffset.Now);
                return dateTimeOffset != null ?dateTimeOffset.Value.DateTime : null;
            }
            catch (Exception e)
            {
                throw new ServiceException(e.Message);
            }
        }
    }
}
