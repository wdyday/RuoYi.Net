using RuoYi.Framework.Components;
using Microsoft.Extensions.DependencyInjection;

namespace System;

/// <summary>
/// 服务组件依赖接口
/// </summary>
public interface IServiceComponent : IComponent
{
    /// <summary>
    /// 装载服务
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="componentContext">组件上下文</param>
    void Load(IServiceCollection services, ComponentContext componentContext);
}