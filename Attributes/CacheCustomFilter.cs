using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static CustomAttributes.HelpMethods;
using System.Text.Json;
using CustomAttributes.ServiceClasses;

namespace CustomAttributes
{
    public class CacheCustomFilter : ActionFilterAttribute, IActionFilter
    {
        ConcurrentDictionary<Type, PropertyInfo[]> cache = new();

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
                return;

            var errorsFromMyAttributesValidation = context.ActionArguments
                    .Select(x => { 
                        var type = x.Value!.GetType();
                        return new ObjectAndTypeWithProp(x.Value, type, cache.GetOrAdd(
                                                                         type, k => (GetPropsWithCustomAttributes(type))));})
                    .Where(x => x.PropertyInfos.Any())
                    .Select(x => new ValidationResult(Validate(x.Value, x.PropertyInfos, out string[] msg), msg))
                    .Where(x => x.IsInvalid)
                    .Select(x => x.ErrMsg)
                    .ToList();

            if (errorsFromMyAttributesValidation.Count < 1)
                return;
            
            var jsonResult = JsonSerializer.Serialize(
                new MyError { ErrorCode = 477, ErrorMessages = errorsFromMyAttributesValidation });

            var result = new ContentResult { Content = jsonResult };
            result.ContentType = "text/json";
            result.StatusCode = 477;
            context.Result = result;
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
