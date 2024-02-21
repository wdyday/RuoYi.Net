﻿namespace RuoYi.Framework.DependencyInjection;

/// <summary>
/// 命名服务提供器
/// </summary>
/// <typeparam name="TService">目标服务接口</typeparam>
public interface INamedServiceProvider<TService>
    where TService : class
{
    /// <summary>
    /// 根据服务名称获取服务
    /// </summary>
    /// <param name="serviceName">服务名称</param>
    /// <returns></returns>
    TService GetService(string serviceName);

    /// <summary>
    /// 根据服务名称获取服务
    /// </summary>
    /// <typeparam name="ILifetime">服务生存周期接口，<see cref="ITransient"/>，<see cref="IScoped"/>，<see cref="ISingleton"/></typeparam>
    /// <param name="serviceName">服务名称</param>
    /// <returns></returns>
    TService GetService<ILifetime>(string serviceName)
        where ILifetime : IPrivateDependency;

    /// <summary>
    /// 根据服务名称获取服务
    /// </summary>
    /// <param name="serviceName">服务名称</param>
    /// <returns></returns>
    TService GetRequiredService(string serviceName);

    /// <summary>
    /// 根据服务名称获取服务
    /// </summary>
    /// <typeparam name="ILifetime">服务生存周期接口，<see cref="ITransient"/>，<see cref="IScoped"/>，<see cref="ISingleton"/></typeparam>
    /// <param name="serviceName">服务名称</param>
    /// <returns></returns>
    TService GetRequiredService<ILifetime>(string serviceName)
        where ILifetime : IPrivateDependency;
}