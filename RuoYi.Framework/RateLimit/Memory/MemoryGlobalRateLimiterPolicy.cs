using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Logging;
using RuoYi.Framework.Utils;
using System.Threading.RateLimiting;

namespace RuoYi.Framework.RateLimit.Memory
{
    public class MemoryGlobalRateLimiterPolicy : IRateLimiterPolicy<string>
    {
        private readonly Func<OnRejectedContext, CancellationToken, ValueTask>? _onRejected;
        private readonly ILogger<MemoryGlobalRateLimiterPolicy> _logger;

        public MemoryGlobalRateLimiterPolicy(ILogger<MemoryGlobalRateLimiterPolicy> logger)
        {
            _logger = logger;
            _onRejected = (context, token) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.");

                return ValueTask.CompletedTask;
            };
        }

        public Func<OnRejectedContext, CancellationToken, ValueTask>? OnRejected { get => _onRejected; }

        public RateLimitPartition<string> GetPartition(HttpContext httpContext)
        {
            var ip = IpUtils.GetIpAddr();
            var rateLimitConfig = App.GetConfig<GlobalLimitConfig>(nameof(GlobalLimitConfig));
            var permitLimit = rateLimitConfig?.PermitLimit ?? 50; // 并发最大数量
            var window = rateLimitConfig?.Window ?? 1; // 默认一秒

            _logger.LogInformation($"IP: {ip} | PermitLimit: {permitLimit} | Window: {window}");

            // 固定窗口限流
            return RateLimitPartition.GetFixedWindowLimiter(LimitType.Default, key => new FixedWindowRateLimiterOptions
            {
                PermitLimit = permitLimit,
                Window = TimeSpan.FromSeconds(window)
            });
        }
    }
}
