using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sol3.Profiles;
using Attributes;
using System.Text.Json;

namespace Sol3.Binder
{
    public class CustomBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            string valueFromBody = string.Empty;

            using (var sr = new StreamReader(bindingContext.HttpContext.Request.Body)) // .ActionContext
            {
                valueFromBody = sr.ReadToEnd();
            }

            if (string.IsNullOrEmpty(valueFromBody))
            {
                throw new ArgumentNullException(nameof(bindingContext));//return Task.CompletedTask;
            }
            var vol = typeof(CreateTankDTO).GetProperty("Volume");
            var maxVol = typeof(CreateTankDTO).GetProperty("Maxvolume");
            var volAttr = (AllowedRangeAttribute?)Attribute.GetCustomAttribute(vol!, typeof(AllowedRangeAttribute));
            var maxVolAttr = (AllowedRangeAttribute?)Attribute.GetCustomAttribute(maxVol!, typeof(AllowedRangeAttribute));

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var result = JsonSerializer.Deserialize<CreateTankDTO>(valueFromBody, options);

            
            if (result.Volume < volAttr!.Min || result.Volume > volAttr!.Max)
            {
                return Task.CompletedTask;
            }
            if (result.Maxvolume < maxVolAttr!.Min || result.Maxvolume > maxVolAttr!.Max)
            {
                return Task.CompletedTask;
            }
            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }
    }
}

