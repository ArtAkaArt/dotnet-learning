using System.Reflection;
using System.Text;

namespace CustomAttributes
{
    internal class HelpMethods
    {
        internal static PropertyInfo[] GetPropsWithCustomAttributes(Type type)
        {
            return type.GetProperties().Where(x => x.GetCustomAttributes(typeof(AllowedRangeAttribute)).Any()).ToArray();
        }
        internal static bool Validate(object argument, PropertyInfo[] props, out string[] ErrorMsg)
        {
            var errors = new StringBuilder();
            foreach (var prop in props)
            {
                var attr = (AllowedRangeAttribute?)prop.GetCustomAttribute(typeof(AllowedRangeAttribute));
                var value = prop.GetValue(argument);
                if (value is not int @int)
                    errors.Append($"{prop.Name} is not an Int in object {argument}.\n");
                else if (!attr.IsValid(@int))
                    errors.Append($"Invalid value of property {prop.Name} in object {argument}.\n");
            }
            ErrorMsg = errors.ToString().Split("\n", StringSplitOptions.RemoveEmptyEntries);
            return ErrorMsg.Length > 0;
        }
    }
}
