using AspectCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.HttpOverrides;
using Newtonsoft.Json;
using RuoYi.Admin.Authorization;
using RuoYi.Common.Files;
using RuoYi.Framework.Cache;
using RuoYi.Framework.Filters;
using RuoYi.Framework.RateLimit;

namespace RuoYi.Admin
{
    [AppStartup(10000)]
    public class Startup: AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
         {
            services.AddConsoleFormatter();
            services.AddCorsAccessor();

            // jwt 鉴权
            services.AddRyJwt();
            // 捕获全局异常
            services.AddMvc(opt =>
            {
                opt.Filters.Add(typeof(GlobalExceptionFilter));
            });

            services.AddControllersWithViews()
                // NewtonsoftJson 
                .AddNewtonsoftJson(options =>
                {
                    // 忽略循环引用
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";

                    // 忽略所有 null 属性
                    //options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    // long 类型序列化时转 string, 防止 JavaScript 出现精度溢出问题
                    //options.SerializerSettings.Converters.AddLongTypeConverters();
                })
                .AddInject(options =>
                {
                    // 不启用全局验证: GlobalEnabled = false
                    options.ConfigureDataValidation(options => { options.GlobalEnabled = false; });
                })
                ;

            // 参数验证返回值处理
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    var msg = string.Join(";", context.ModelState.Select(x => x.Value.Errors?.FirstOrDefault().ErrorMessage).ToList());
                    return new JsonResult(AjaxResult.Error(msg));
                };
            });

            // 如果服务器端使用了 nginx/iis 等反向代理工具，可添加以下代码配置：
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;

                // 若上面配置无效可尝试下列代码，比如在 IIS 中
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });

            #region 日志
            // 全局启用 LoggingMonitor 功能
            services.AddMonitorLogging();

            // 日志
            Array.ForEach(new[] { LogLevel.Information, LogLevel.Warning, LogLevel.Error }, logLevel =>
            {
                services.AddFileLogging("logs/application-{1}-{0:yyyy}-{0:MM}-{0:dd}.log", options =>
                {
                    options.FileNameRule = fileName => string.Format(fileName, DateTime.UtcNow, logLevel.ToString());
                    options.WriteFilter = logMsg => logMsg.LogLevel == logLevel;
                });
            });
            #endregion

            // 远程请求
            services.AddRemoteRequest();

            // SqlSugar
            services.AddSqlSugarScope();

            // Cache
            services.AddCache();

            // SignalR
            services.AddSignalR();

            // captcha
            services.AddLazyCaptcha();

            // 自定义拦截器 (AspectCore)
            services.ConfigureDynamicProxy();

            // 限流 https://github.com/cristipufu/aspnetcore-redis-rate-limiting/tree/master
            services.AddConcurrencyLimiter();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // 如果服务器端使用了 nginx/iis 等反向代理工具: 注意在 Configure 最前面配置
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            //app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRyStaticFiles(env); 

            app.UseRouting();

            app.UseCorsAccessor();

            app.UseAuthentication();
            app.UseAuthorization();

            // 注入基础中间件
            app.UseInject();

            // 限流
            app.UseRateLimiter();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}