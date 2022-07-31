using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Attributes
{
    public class AttributeFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ModelState.IsValid)
                await next();
            else
            {
                var objectsWithMyAttr = context.ActionArguments
                    .Select(x => new { dto = x.Value, propoperties = x.Value
                                                            .GetType()
                                                            .GetProperties()})
//не будет ли читабельнее .Select(x => new { obj = x.Value, prop = x.Value.GetType().GetProperties()}) ?
                    .Where(x => x.propoperties
                                 .Where(x => x.GetCustomAttributes(typeof(AllowedRangeAttribute)) is not null)
                                     .Any());
                var errorsMessage = new StringBuilder();
                foreach (var obj in objectsWithMyAttr)
                {
                    foreach (var property in obj.propoperties)
                    {
                        var attr = (AllowedRangeAttribute?)property.GetCustomAttribute(typeof(AllowedRangeAttribute));
                        if (attr is null)
                            continue;
                        var value = property.GetValue(obj.dto);
                        if (value is not int @int) // модель будет не валидна (строка  13), если аттрибут будет не на int
                        {
                            errorsMessage.Append($"{property.Name} is not an Int in object {obj.dto.GetType().Name} \n");
                        }
                        else if (@int < attr.Min || @int > attr.Max)
                        {
                            errorsMessage.Append($"Invalid value of property {property.Name} in object {obj.dto.GetType().Name} \n");
                        }
                    }
                }
                if (errorsMessage.Length == 0)
                    await next();
                else
                {
                    var result = new ContentResult { Content = errorsMessage.ToString() };
                    result.StatusCode = 477;
                    context.Result = result;
                }
            }
        }
    }
}