using Microsoft.AspNetCore.Builder;
using RuoYi.Framework.Components;

namespace System;

/// <summary>
/// Web 组件依赖接口
/// </summary>
/// <remarks>注意，此时 RuoYi.Framework 还未载入</remarks>
public interface IWebComponent : IComponent
{
    /// <summary>
    /// 装置 Web 应用构建器
    /// </summary>
    /// <remarks>注意，此时 RuoYi.Framework 还未载入</remarks>
    /// <param name="builder"><see cref="WebApplicationBuilder"/></param>
    /// <param name="componentContext">组件上下文</param>
    void Load(WebApplicationBuilder builder, ComponentContext componentContext);
}