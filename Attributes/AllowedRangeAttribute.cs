using System.ComponentModel.DataAnnotations;

namespace Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AllowedRangeAttribute : ValidationAttribute
    {

        public int Min { get; set; }
        public int Max { get; set; }

        public AllowedRangeAttribute(int minValue, int maxValue)
        {
            Min = minValue;
            Max = maxValue;
        }
        public override bool IsValid(object? value)
        {
            if (value is null)
                return false;
            var typeName = value.GetType().Name.ToString();
            switch (typeName)
            {
                case "Int32":
                        if ((int)value < Min || (int)value > Max)
                            return false;
                        return true;
                case "String":
                    if (((string)value).Length < Min || ((string)value).Length > Max)
                        return false;
                    return true;
                default: return false;
            }
        }
    }
}