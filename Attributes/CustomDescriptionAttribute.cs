namespace Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class CustomDescriptionAttribute : Attribute
    {
        public string Description { get; set; }
        public CustomDescriptionAttribute(string description)
        {
            Description = description;
        }
    }
}