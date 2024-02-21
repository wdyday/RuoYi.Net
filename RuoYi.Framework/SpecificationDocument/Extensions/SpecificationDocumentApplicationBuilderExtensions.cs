using RuoYi.Framework;
using RuoYi.Framework.SpecificationDocument;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Microsoft.AspNetCore.Builder;

/// <summary>
/// 规范化文档中间件拓展
/// </summary>
[SuppressSniffer]
public static class SpecificationDocumentApplicationBuilderExtensions
{
    /// <summary>
    /// 添加规范化文档中间件
    /// </summary>
    /// <param name="app"></param>
    /// <param name="routePrefix"></param>
    /// <param name="configureSwagger"></param>
    /// <param name="configureSwaggerUI"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseSpecificationDocuments(this IApplicationBuilder app, string routePrefix = default, Action<SwaggerOptions> configureSwagger = default, Action<SwaggerUIOptions> configureSwaggerUI = default)
    {
        // 判断是否启用规范化文档
        if (App.Settings.InjectSpecificationDocument != true) return app;

        // 配置 Swagger 全局参数
        app.UseSwagger(options => SpecificationDocumentBuilder.Build(options, configureSwagger));

        // 配置 Swagger UI 参数
        app.UseSwaggerUI(options => SpecificationDocumentBuilder.BuildUI(options, routePrefix, configureSwaggerUI));

        // 启用 MiniProfiler组件
        if (App.Settings.InjectMiniProfiler == true) app.UseMiniProfiler();

        return app;
    }
}