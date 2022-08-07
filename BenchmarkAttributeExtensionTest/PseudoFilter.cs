using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Attributes;

namespace Test
{
    public class AttributeFilter
    {
        ConcurrentDictionary<Type, PropertyInfo[]> bag = new();
        public AttributeFilter(ConcurrentDictionary<Type, PropertyInfo[]> bag)
        {
            this.bag = bag;
        }
        public void OnActionExecutionAsync(PseudoContext context)
        {
            var objectsWithMyAttr = context.ActionArguments
                .Select(x => new
                {
                    dto = x.Value,
                    propoperties = x.Value
                                    .GetType()
                                    .GetProperties()
                })
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
                        errorsMessage.Append($"{property.Name} is not an Int in object {obj.dto.GetType().Name} \n");
                    else if (@int < attr.Min || @int > attr.Max)
                        errorsMessage.Append($"Invalid value of property {property.Name} in object {obj.dto.GetType().Name} \n");
                }
            }
        }
        public void OnActionExecutionAltAsync(PseudoContext context)
        {
            var argsCopy = context.ActionArguments.Select(x => x.Value).ToList();
            var objectsWithMyAttr = new List<object[]>();
            foreach (var obj in context.ActionArguments.Select(x => x.Value))
            {
                if (!bag.TryGetValue(obj!.GetType(), out var attrs))
                    continue;
                argsCopy.Remove(obj);
                objectsWithMyAttr.Add(new object[] { obj, attrs });
            }

            var restObjesctsWithAttr =
                argsCopy.Select(x => new object[] { x, x.GetType().GetProperties() })
                        .Where(x => ((PropertyInfo[])x[1])
                                     .Where(x => x.GetCustomAttributes(typeof(AllowedRangeAttribute)) is not null)
                        .Any());

            objectsWithMyAttr.AddRange(restObjesctsWithAttr);

            var errorsMessage = new StringBuilder();
            foreach (var obj in objectsWithMyAttr)
            {
                var props = (PropertyInfo[])obj[1];
                foreach (var property in props)
                {
                    var attr = (AllowedRangeAttribute?)property.GetCustomAttribute(typeof(AllowedRangeAttribute));
                    if (attr is null)
                        continue;
                    bag.TryAdd(obj[0].GetType(), props);
                    var value = property.GetValue(obj[0]);
                    if (value is not int @int) // модель будет не валидна (строка  13), если аттрибут будет не на int
                        errorsMessage.Append($"{property.Name} is not an Int in object {obj[0]} \n");
                    else if (@int < attr.Min || @int > attr.Max)
                        errorsMessage.Append($"Invalid value of property {property.Name} in object {obj[0]} \n");
                }
            }
        }
    }
}