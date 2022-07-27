using System.ComponentModel.DataAnnotations;

namespace Attributes
{
    /// <summary>
    /// Позволяет проверять поля типов Int на попдание их значений в заданный Range значений.
    /// При применении аттрибута к другим типам полей, проверка не будет пройдена.
    /// </summary>
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
            if (typeName == "Int32" || typeName == "UInt32")
            {
                if ((int)value < Min || (int)value > Max)
                    return false;
                return true;
            }
            return false;
        }
    }
}