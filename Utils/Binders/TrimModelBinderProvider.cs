using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Hubtel.Api.Utils.Binders;

public class TrimModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (context.Metadata.IsComplexType) return null;

        if (context.Metadata.ModelType == typeof(string)) return new TrimModelBinder();

        return null;
    }
}