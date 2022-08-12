using CustomAttributes;
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
    public class Obj7
    {
        [AllowedRange(0, 1000)]
        public int? Int1 { get; set; }
        [AllowedRange(0, 1000)]
        public int? Int2 { get; set; }
        [AllowedRange(0, 1000)]
        public int? Int3 { get; set; }
        public int? Int4 { get; set; }
        public bool? Bool { get; set; }
        public double? Double { get; set; }
        public float? Float { get; set; }
        public int? Int5 { get; set; }

    }
    public class Obj8
    {
        [AllowedRange(500, 400)] //why not
        public int? Int1 { get; set; }
        [AllowedRange(-1, 100)]
        public int? Int2 { get; set; }
        [AllowedRange(150, 1000)]
        public int? Int3 { get; set; }
        public int? Int4 { get; set; }
        [AllowedRange(0, 1000)]
        public bool? Bool { get; set; }
        public double? Double { get; set; }
        public float? Float { get; set; }
        public int? Int5 { get; set; }
    }
    public class Obj9
    {
        [AllowedRange(0, 0)]
        public int? Int1 { get; set; }
        [AllowedRange(0, 1000)]
        public int? Int2 { get; set; }
        [AllowedRange(-500, 0)]
        public int? Int3 { get; set; }
        public int? Int4 { get; set; }
        [AllowedRange(0, 1000)]
        public bool? Bool { get; set; }
        public double? Double { get; set; }
        public float? Float { get; set; }
        public int? Int5 { get; set; }
    }
    public class Obj10
    {
        [AllowedRange(-10, 1000)]
        public int? Int1 { get; set; }
        [AllowedRange(77, 987)]
        public int? Int2 { get; set; }
        [AllowedRange(0, 1000)]
        public int? Int3 { get; set; }
        public int? Int4 { get; set; }
        public bool? Bool { get; set; }
        public double? Double { get; set; }
        public float? Float { get; set; }
        public int? Int5 { get; set; }
    }
}
