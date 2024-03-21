using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Logging;
using RedisRateLimiting;
using RuoYi.Framework.Utils;
using StackExchange.Redis;
using System.Threading.RateLimiting;

namespace RuoYi.Framework.RateLimit.Redis
{
    public class GlobalRateLimiterPolicy : IRateLimiterPolicy<string>
    {
        private readonly Func<OnRejectedContext, CancellationToken, ValueTask>? _onRejected;
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly ILogger<GlobalRateLimiterPolicy> _logger;

        public GlobalRateLimiterPolicy(
            IConnectionMultiplexer connectionMultiplexer,
            ILogger<GlobalRateLimiterPolicy> logger)
        {
            _connectionMultiplexer = connectionMultiplexer;
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
            return RedisRateLimitPartition.GetFixedWindowRateLimiter(LimitType.Default, key => new RedisFixedWindowRateLimiterOptions
            {
                PermitLimit = permitLimit,
                Window = TimeSpan.FromSeconds(window),
                ConnectionMultiplexerFactory = () => _connectionMultiplexer
            });
        }
    }
}
