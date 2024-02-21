using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace RuoYi.Framework.AspNetCore;

/// <summary>
/// 模型转换绑定器接口
/// </summary>
public interface IModelConvertBinder
{
    /// <summary>
    /// 模型绑定转换方法
    /// </summary>
    /// <param name="bindingContext"></param>
    /// <param name="metadata"></param>
    /// <param name="valueProviderResult"></param>
    /// <param name="extras"></param>
    /// <returns></returns>
    object ConvertTo(ModelBindingContext bindingContext, DefaultModelMetadata metadata, ValueProviderResult valueProviderResult, object extras = default);
}