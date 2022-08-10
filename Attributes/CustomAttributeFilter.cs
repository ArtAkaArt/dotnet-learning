using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static CustomAttributes.HelpMethods;
using System.Text.Json;
using CustomAttributes.ServiceClasses;

namespace CustomAttributes
{
    public class CustomAttributeFilter : ActionFilterAttribute, IAsyncActionFilter 
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ModelState.IsValid)
            {
                await next();
                return;
            }
            var errorsFromMyAttributesValidation = context.ActionArguments
                    .Select(x => new ObjectAndType(x.Value!, x.Value!.GetType()))
                    .Select(x => new ObjectAndTypeWithProp(x.Value, x.MyType, GetPropsWithCustomAttributes(x.MyType)))
                    .Where(x => x.PropertyInfos.Any())
                    .Select(x => new ValidationResult(Validate(x.Value, x.PropertyInfos, out string Msg), Msg))
                    .Where(x => x.IsInvalid)
                    .Select(x => x.ErrMsg);
            if (errorsFromMyAttributesValidation.Any())
            {
                await next();
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