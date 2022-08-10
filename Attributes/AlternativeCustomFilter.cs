using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static CustomAttributes.HelpMethods;
using System.Text.Json;
using CustomAttributes.ServiceClasses;

namespace CustomAttributes
{
    public class AlternativeCustomFilter : ActionFilterAttribute, IActionFilter
    {
        ConcurrentDictionary<Type, PropertyInfo[]> cache = new();

        public override void  OnActionExecuted(ActionExecutedContext context)
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }

            var errorsFromMyAttributesValidation = context.ActionArguments
                    .Select(x => new ObjectAndType(x.Value!, x.Value!.GetType()))
                    .Select(x => new ObjectAndTypeWithProp(x.Value, x.MyType, cache.GetOrAdd(
                                                                        x.MyType, k => (GetPropsWithCustomAttributes(x.MyType)))))
                    .Where(x => x.PropertyInfos.Any())
                    .Select(x => new ValidationResult(Validate(x.Value, x.PropertyInfos, out string Msg), Msg))
                    .Where(x => x.IsInvalid)
                    .Select(x => x.ErrMsg);

            if (!errorsFromMyAttributesValidation.Any())
            {
                return;
            }
            var jsonResult = JsonSerializer.Serialize(new MyError { ErrorCode = 477, ErrprMessages = errorsFromMyAttributesValidation });

            var result = new ContentResult { Content = jsonResult };
            result.ContentType = "text/json";
            result.StatusCode = 477;
            context.Result = result;
        }
    }
}
