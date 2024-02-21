using Microsoft.OpenApi.Models;
using RuoYi.Framework.ApiController;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RuoYi.Framework.SpecificationDocument;

/// <summary>
/// 标签文档排序/注释拦截器
/// </summary>
[SuppressSniffer]
public class TagsOrderDocumentFilter : IDocumentFilter
{
    /// <summary>
    /// 配置拦截
    /// </summary>
    /// <param name="swaggerDoc"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        swaggerDoc.Tags = Penetrates.ControllerOrderCollection
            .Where(u => SpecificationDocumentBuilder.GetControllerGroups(u.Value.Item3).Any(c => c.Group == context.DocumentName))
            .OrderByDescending(u => u.Value.Item2)
            .ThenBy(u => u.Key)
            .Select(c => new OpenApiTag
            {
                Name = c.Value.Item1,
                Description = swaggerDoc.Tags.FirstOrDefault(m => m.Name == c.Key)?.Description
            }).ToList();
    }
}