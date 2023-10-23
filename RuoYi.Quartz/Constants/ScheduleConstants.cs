namespace RuoYi.Quartz.Constants
{
    public class ScheduleConstants
    {
        public const string TASK_CLASS_NAME = "TASK_CLASS_NAME";

        /** 执行目标key */
        public const string TASK_PROPERTIES = "TASK_PROPERTIES";

        /** 默认 */
        public const string MISFIRE_DEFAULT = "0";

        /** 立即触发执行 */
        public const string MISFIRE_IGNORE_MISFIRES = "1";

        /** 触发一次执行 */
        public const string MISFIRE_FIRE_AND_PROCEED = "2";

        /** 不触发立即执行 */
        public const string MISFIRE_DO_NOTHING = "3";

        // 定时任务白名单配置（仅允许访问的命名空间，如其他需要可以自行添加）
        public static string[] JOB_WHITELIST_STR = new string[] { "RuoYi" };

        // 定时任务违规的字符
        public static string[] JOB_ERROR_STR = new string[] { "RuoYi.Framework", "RuoYi.Data", "RuoYi.Common" };
    }
}
