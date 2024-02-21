using RuoYi.Framework.DataValidation;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RuoYi.Framework;

/// <summary>
/// AddInject 配置选项
/// </summary>
public sealed class AddInjectOptions
{
    /// <summary>
    /// 配置 Swagger Gen
    /// </summary>
    /// <param name="configure"></param>
    public void ConfigureSwaggerGen(Action<SwaggerGenOptions> configure)
    {
        SwaggerGenConfigure = configure;
    }

    /// <summary>
    /// 配置 DataValidation
    /// </summary>
    /// <param name="configure"></param>
    public void ConfigureDataValidation(Action<DataValidationOptions> configure)
    {
        DataValidationConfigure = configure;
    }

    /// <summary>
    /// Swagger Gen 配置
    /// </summary>
    internal static Action<SwaggerGenOptions> SwaggerGenConfigure { get; private set; }

    /// <summary>
    /// DataValidation 配置
    /// </summary>
    internal static Action<DataValidationOptions> DataValidationConfigure { get; private set; }
}