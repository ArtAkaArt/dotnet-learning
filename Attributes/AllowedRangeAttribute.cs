using System.ComponentModel.DataAnnotations;

namespace CustomAttributes
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
            if (value is int @int)
            {
                return !(@int < Min || @int > Max);
            }
            return false;
        }
    }
}