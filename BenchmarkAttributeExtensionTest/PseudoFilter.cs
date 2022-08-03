﻿using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Attributes;

namespace Test
{
    public class AttributeFilter
    {
        ConcurrentDictionary<object, PropertyInfo[]> bag = new();
        public AttributeFilter(ConcurrentDictionary<object, PropertyInfo[]> bag)
        {
            this.bag = bag;
        }
        public void OnActionExecutionAsync(PseudoContext context)
        {
            var objectsWithMyAttr = context.ActionArguments
                .Select(x => new {
                    dto = x.Value,
                    propoperties = x.Value
                                    .GetType()
                                    .GetProperties()})
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
            var objectsWithMyAttr = new Dictionary<object, PropertyInfo[]>();
            foreach (var obj in context.ActionArguments.Select(x => x.Value).ToList())
            {
                foreach (var arg in bag)
                {
                    if (arg.Key.GetType() == obj?.GetType())
                    {
                        argsCopy.Remove(obj);
                        objectsWithMyAttr.Add(arg.Key, arg.Value);
                    }
                }
            }

            var restObjesctsWithAttr =
                    argsCopy.Select(x => new { dto = x, propoperties = x!.GetType().GetProperties() })
                            .Where(x => x.propoperties
                                         .Where(x => x.GetCustomAttributes(typeof(AllowedRangeAttribute)) is not null)
                            .Any())
                            .ToDictionary(x => x.dto!, x => x.propoperties);

            restObjesctsWithAttr.ToList().ForEach(x => objectsWithMyAttr.Add(x.Key, x.Value));

            var errorsMessage = new StringBuilder();
            foreach (var obj in objectsWithMyAttr)
            {
                foreach (var property in obj.Value)
                {
                    var attr = (AllowedRangeAttribute?)property.GetCustomAttribute(typeof(AllowedRangeAttribute));
                    if (attr is null)
                        continue;
                    bag.TryAdd(obj.Key, obj.Value);
                    var value = property.GetValue(obj.Key);
                    if (value is not int @int) // модель будет не валидна (строка  13), если аттрибут будет не на int
                        errorsMessage.Append($"{property.Name} is not an Int in object {obj.Key.GetType().Name} \n");
                    else if (@int < attr.Min || @int > attr.Max)
                        errorsMessage.Append($"Invalid value of property {property.Name} in object {obj.Key.GetType().Name} \n");
                }
            }
        }
    }
}