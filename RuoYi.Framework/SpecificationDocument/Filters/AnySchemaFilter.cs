using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RuoYi.Framework.SpecificationDocument;

/// <summary>
/// 修正 规范化文档 object schema，统一显示为 any
/// </summary>
/// <remarks>相关 issue：https://github.com/swagger-api/swagger-codegen-generators/issues/692 </remarks>
[SuppressSniffer]
public class AnySchemaFilter : ISchemaFilter
{
    /// <summary>
    /// 实现过滤器方法
    /// </summary>
    /// <param name="model"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiSchema model, SchemaFilterContext context)
    {
        var type = context.Type;

        if (type == typeof(object))
        {
            model.AdditionalPropertiesAllowed = false;
        }
    }
}