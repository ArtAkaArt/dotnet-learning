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
        public override bool IsValid(object value)
        {
            if ((int)value < Min || (int)value > Max) return false;
            return true;
        }
    }
}