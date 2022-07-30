using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Attributes
{

    public class AttributeFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context,
                                    ActionExecutionDelegate next)
        {
            if (context.ModelState.IsValid)
                await next();
            var objectsWithMyAttr = context.ActionArguments
                .Select(x => x.Value)
                .Where(x => x.GetType()
                    .GetProperties()
                    .Where(x => x.GetCustomAttributes(typeof(AllowedRangeAttribute)) is not null)
                .Any());
            var errorsCount = 0;
            var errorsMessage = new StringBuilder();
            foreach (var obj in objectsWithMyAttr)
            {
                var properties = obj.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var attr = (AllowedRangeAttribute?)property.GetCustomAttribute(typeof(AllowedRangeAttribute));
                    if (attr is null)
                        continue;
                    var value = property.GetValue(obj);
                    if ((int)value! < attr.Min || (int)value > attr.Max)
                    {
                        errorsCount++;
                        errorsMessage.Append($"Invalid value of property {property.Name} in object {obj.GetType().Name} \n");
                    }
                }
            }
            if (errorsCount <= 0)
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