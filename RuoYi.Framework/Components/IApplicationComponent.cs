using RuoYi.Framework.Components;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace System;

/// <summary>
/// 应用中间件接口
/// </summary>
public interface IApplicationComponent : IComponent
{
    /// <summary>
    /// 装置中间件
    /// </summary>
    /// <param name="app"><see cref="IApplicationBuilder"/></param>
    /// <param name="env"><see cref="IWebHostEnvironment"/></param>
    /// <param name="componentContext">组件上下文</param>
    void Load(IApplicationBuilder app, IWebHostEnvironment env, ComponentContext componentContext);
}