using AspectCore.Extensions.DependencyInjection;
using RuoYi.Web.Entry;

internal class Program
{
    private static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(serverOptions =>
                {
                    // Set properties and call methods on options
                    //serverOptions.Limits.MaxRequestBodySize = 50 * 1024 * 1024;
                    serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(1);
                    serverOptions.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(1);
                })
               .Inject()
               .UseStartup<Startup>();
            })
            .UseServiceProviderFactory(new DynamicProxyServiceProviderFactory());
    }
}