using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace RuoYi.Framework.ModelBinders;

public class IntArrayModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (context.Metadata.ModelType == typeof(long[]) || context.Metadata.ModelType == typeof(int[]))
        {
            return new BinderTypeModelBinder(typeof(IntArrayModelBinder));
        }

        return null;
    }
}
