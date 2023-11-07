namespace RuoYi.Framework.RateLimit
{
    public class LimitType
    {
        /// <summary>
        /// 全局限流
        /// </summary>
        public const string Default = "GlobalLimiter";

        /// <summary>
        /// 按IP限流
        /// </summary>
        public const string IP = "IpRateLimiter";
    }
}
