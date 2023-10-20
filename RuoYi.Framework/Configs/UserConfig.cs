namespace RuoYi.Framework.Configs
{
    public class UserConfig
    {
        /// <summary>
        /// 密码最大错误次数
        /// </summary>
        public int MaxRetryCount { get; set; }

        /// <summary>
        /// 密码锁定时间（默认10分钟）
        /// </summary>
        public int LockTime { get; set; }
    }
}
