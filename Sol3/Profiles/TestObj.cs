using Attributes;
namespace Sol3.Profiles
{
    public class Obj1
    {
        [AllowedRange(200, 1000)]
        public int? Vol { get; set; }
    }
    public class Obj2
    {
        [AllowedRange(0, 1000)]
        public int? Vol { get; set; }
    }
    public class Obj3
    {
        public int? Vol { get; set; }
    }
    public class Obj4
    {
        public int? Vol { get; set; }
    }
    public class Obj5
    {
        [AllowedRange(200, 1000)]
        public int? Vol { get; set; }
    }
    public class Obj6
    {
        [AllowedRange(0, 1000)]
        public int? Vol { get; set; }
    }
}
