using System.Reflection;

namespace CustomAttributes.ServiceClasses
{
    internal record ObjectAndType
    {
        internal object Value { get; set; }
        internal Type MyType { get; set; }
        internal ObjectAndType(object name, Type Type)
        {
            Value = name;
            MyType = Type;
        }
    }
    internal record ObjectAndTypeWithProp
    {
        internal object Value { get; set; }
        internal Type MyType { get; set; }
        internal PropertyInfo[] PropertyInfos { get; set; }

        internal ObjectAndTypeWithProp(object name, Type Type, PropertyInfo[] props)
        {
            Value = name;
            MyType = Type;
            PropertyInfos = props;
        }
    }
    internal record struct ValidationResult
    {
        internal bool IsInvalid { get; set; }
        internal string ErrMsg { get; set; }
        internal ValidationResult(bool isInvalid, string errMsg)
        {
            IsInvalid = isInvalid;
            ErrMsg = errMsg;
        }
    }
    internal record struct MyError
    {
        public int ErrorCode { get; set; }
        public IEnumerable<string> ErrprMessages { get; set; }
    }
}
