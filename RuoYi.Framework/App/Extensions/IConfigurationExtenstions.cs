using RuoYi.Framework;

namespace Microsoft.Extensions.Configuration;

/// <summary>
/// <see cref="IConfiguration"/> 拓展
/// </summary>
public static class IConfigurationExtenstions
{
    /// <summary>
    /// 刷新配置对象
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IConfiguration Reload(this IConfiguration configuration)
    {
        if (App.RootServices == null) return configuration;

        var newConfiguration = App.GetService<IConfiguration>(App.RootServices);
        InternalApp.Configuration = newConfiguration;

        return newConfiguration;
    }
}