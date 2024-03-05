using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace RuoYi.Framework.RateLimit
{
    /// <summary>
    /// 并发控制, 参考
    /// https://github.com/cristipufu/aspnetcore-redis-rate-limiting/tree/master
    /// </summary>
    public static class RateLimitExtensions
    {
        public static IServiceCollection AddConcurrencyLimiter(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                // 全局 并发限流
                options.AddPolicy<string, GlobalRateLimiterPolicy>(LimitType.Default);

                // ip 并发限流
                options.AddPolicy<string, IpRateLimiterPolicy>(LimitType.IP);
            });

            return services;
        }
    }
}
