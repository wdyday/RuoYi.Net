using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace RuoYi.Framework;

/// <summary>
/// UseInject 配置选项
/// </summary>
public sealed class UseInjectOptions
{
    /// <summary>
    /// 配置 Swagger
    /// </summary>
    /// <param name="configure"></param>
    public void ConfigureSwagger(Action<SwaggerOptions> configure)
    {
        SwaggerConfigure = configure;
    }

    /// <summary>
    /// 配置 Swagger UI
    /// </summary>
    /// <param name="configure"></param>
    public void ConfigureSwaggerUI(Action<SwaggerUIOptions> configure)
    {
        SwaggerUIConfigure = configure;
    }

    /// <summary>
    /// Swagger 配置
    /// </summary>
    internal static Action<SwaggerOptions> SwaggerConfigure { get; private set; }

    /// <summary>
    /// Swagger UI 配置
    /// </summary>
    internal static Action<SwaggerUIOptions> SwaggerUIConfigure { get; private set; }
}