using SqlSugar;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// SqlSugar 拓展类
/// </summary>
public static class SqlSugarServiceCollectionExtensions
{
    /// <summary>
    /// 添加 SqlSugar 拓展
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    /// <param name="buildAction"></param>
    /// <returns></returns>
    public static IServiceCollection AddSqlSugarClient(this IServiceCollection services, ConnectionConfig config, Action<ISqlSugarClient> buildAction = default)
    {
        return services.AddSqlSugarClient(new ConnectionConfig[] { config }, buildAction);
    }

    /// <summary>
    /// 添加 SqlSugar 拓展
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configs"></param>
    /// <param name="buildAction"></param>
    /// <returns></returns>
    public static IServiceCollection AddSqlSugarClient(this IServiceCollection services, ConnectionConfig[] configs, Action<ISqlSugarClient> buildAction = default)
    {
        // 注册 SqlSugar 客户端
        services.AddScoped<ISqlSugarClient>(u =>
        {
            var sqlSugarClient = new SqlSugarClient(configs.ToList());
            buildAction?.Invoke(sqlSugarClient);

            return sqlSugarClient;
        });

        // 注册非泛型仓储
        services.AddScoped<ISqlSugarRepository, SqlSugarRepository>();

        // 注册 SqlSugar 仓储
        services.AddScoped(typeof(ISqlSugarRepository<>), typeof(SqlSugarRepository<>));

        return services;
    }

    // 注册 SqlSugarScope
    public static IServiceCollection AddSqlSugarScope(this IServiceCollection services, ConnectionConfig[] configs, Action<SqlSugarScope> buildAction = default)
    {
        // 注册 SqlSugar 
        services.AddSingleton<ISqlSugarClient>(u =>
        {
            // SqlSugarScope 必须使用单例
            var sqlSugarScope = new SqlSugarScope(configs.ToList());
            buildAction?.Invoke(sqlSugarScope);
            return sqlSugarScope;
        });

        // 注册非泛型仓储
        services.AddScoped<ISqlSugarRepository, SqlSugarRepository>();

        // 注册 SqlSugar 仓储
        services.AddScoped(typeof(ISqlSugarRepository<>), typeof(SqlSugarRepository<>));

        return services;
    }
}