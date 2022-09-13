namespace MyORM
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class TableBindingNameAttribute : Attribute
    {
        string TableName { get; set; }
        public TableBindingNameAttribute(string name) => TableName = name;
        public string GetName() => TableName;
    }
}
