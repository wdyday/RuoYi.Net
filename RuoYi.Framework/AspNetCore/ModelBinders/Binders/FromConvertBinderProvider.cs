﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using RuoYi.Framework.AspNetCore;
using RuoYi.Framework.DependencyInjection;
using System.Collections.Concurrent;

namespace RuoYi.Framework.SensitiveDetection;

/// <summary>
/// [FromConvert] 模型绑定提供器
/// </summary>
[SuppressSniffer]
public class FromConvertBinderProvider : IModelBinderProvider
{
    /// <summary>
    /// 定义模型绑定转换器集合
    /// </summary>
    private readonly ConcurrentDictionary<Type, Type> _modelBinderConverts;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="modelBinderConverts">定义模型绑定转换器集合</param>
    public FromConvertBinderProvider(ConcurrentDictionary<Type, Type> modelBinderConverts)
    {
        _modelBinderConverts = modelBinderConverts;
    }

    /// <summary>
    /// 返回自定义绑定器
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        // 判断是否定义 [FromConvert] 特性
        if (context.Metadata is DefaultModelMetadata actMetadata
            && actMetadata.Attributes.ParameterAttributes != null
            && actMetadata.Attributes.ParameterAttributes.Count > 0
            && actMetadata.Attributes.ParameterAttributes.Any(u => u.GetType() == typeof(FromConvertAttribute)))
        {
            return new FromConvertBinder(_modelBinderConverts);
        }

        return null;
    }
}