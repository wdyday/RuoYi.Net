namespace RuoYi.Framework.RateLimit
{
    public class IpRateLimitConfig
    {
        /// <summary>
        /// 最多并发的请求数。该值必须 > 0
        /// </summary>
        public int PermitLimit { get; set; }

        /// <summary>
        /// 窗口大小，即时间长度(秒)
        /// </summary>
        public int Window { get; set; }
    }
}
