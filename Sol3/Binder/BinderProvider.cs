using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sol3.Profiles;
namespace Sol3.Binder
{
    public class CustomBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(CreateTankDTO))
                return new CustomBinder();
            return null;
        }
    }
}

