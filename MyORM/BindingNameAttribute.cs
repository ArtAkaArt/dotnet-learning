namespace MyORM
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class BindingNameAttribute : Attribute
    {
        string? TableName { get; set; }
        public BindingNameAttribute(string name) => TableName = name;
    }
}
