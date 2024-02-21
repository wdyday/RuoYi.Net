﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace RuoYi.Framework.SpecificationDocument;

/// <summary>
/// 规范化文档自定义更多功能
/// </summary>
public class ApiActionFilter : IOperationFilter
{
    /// <summary>
    /// 实现过滤器方法
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="context"></param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // 获取方法
        var method = context.MethodInfo;

        // 处理更多描述
        if (method.IsDefined(typeof(ApiDescriptionSettingsAttribute), true))
        {
            var apiDescriptionSettings = method.GetCustomAttribute<ApiDescriptionSettingsAttribute>(true);

            // 添加单一接口描述
            if (!string.IsNullOrWhiteSpace(apiDescriptionSettings.Description))
            {
                operation.Description += apiDescriptionSettings.Description;
            }
        }

        // 处理过时
        if (method.IsDefined(typeof(ObsoleteAttribute), true))
        {
            var deprecated = method.GetCustomAttribute<ObsoleteAttribute>(true);
            if (!string.IsNullOrWhiteSpace(deprecated.Message))
            {
                operation.Description = $"<div>{deprecated.Message}</div>" + operation.Description;
            }
        }
    }
}