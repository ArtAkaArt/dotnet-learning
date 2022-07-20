namespace Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class AllowedRangeAttribute : Attribute
    {
        public int Min { get; set; }
        public int Max { get; set; }

        public AllowedRangeAttribute(int minValue, int maxValue)
        {
            Min = minValue;
            Max = maxValue;
        }
    }
}