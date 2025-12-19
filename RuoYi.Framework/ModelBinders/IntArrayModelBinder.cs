using Microsoft.AspNetCore.Mvc.ModelBinding;
using RuoYi.Framework.Utils;

namespace RuoYi.Framework.ModelBinders;

public class IntArrayModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var modelName = bindingContext.ModelName;

        // Try to fetch the value of the argument by name
        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

        var value = valueProviderResult.FirstValue;

        // Check if the argument value is null or empty
        if (string.IsNullOrEmpty(value) || !StringUtils.IsNumber(value))
        {
            return Task.CompletedTask;
        }

        if (bindingContext.ModelType == typeof(long[]))
        {
            var ids = value.Split(',').Select(id => Convert.ToInt64(id)).ToArray();
            bindingContext.Result = ModelBindingResult.Success(ids);
        }
        else
        {
            var ids = value.Split(',').Select(id => Convert.ToInt32(id)).ToArray();
            bindingContext.Result = ModelBindingResult.Success(ids);
        }

        return Task.CompletedTask;
    }
}
