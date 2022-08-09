using System.Reflection;

namespace CustomAttributes
{
    internal class HelpMethods
    {
        internal static PropertyInfo[] GetPropsWithCustomAttributes(Type type)
        {
            return type.GetProperties().Where(x => x.GetCustomAttributes(typeof(AllowedRangeAttribute)).Any()).ToArray();
        }
        internal static bool Validate(object argument, PropertyInfo[] props, out string ErrorMsg)
        {
            ErrorMsg = "";
            foreach (var prop in props)
            {
                var attr = (AllowedRangeAttribute?)prop.GetCustomAttribute(typeof(AllowedRangeAttribute));
                var value = prop.GetValue(argument);
                if (value is not int @int)
                    ErrorMsg = $"{prop.Name} is not an Int in object {argument}.";
                else if (!attr.IsValid(@int))
                    ErrorMsg = $"Invalid value of property {prop.Name} in object {argument}.";
            }
            return ErrorMsg.Length > 0;
        }
        internal static Type GetRealType(object argument)
        {
            return argument.GetType();
        }
    }
}
