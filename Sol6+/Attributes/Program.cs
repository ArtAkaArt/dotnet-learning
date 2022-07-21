namespace Attributes
{
    public class Program
    {
        public static void Main()
        {
            var tank = new Tank() { Volume = 300, Maxvolume = 500};
            var isOk = VerifyVolume(typeof(Tank), tank);
            Console.WriteLine(isOk);
            WriteAllDescriptions(typeof(Tank));
        }
        static bool VerifyVolume(Type t, Tank tank)
        {
            if (tank is null) return false;
            var vol = t.GetProperty("Volume");
            var mVol = t.GetProperty("Maxvolume");
            var volAttr = (AllowedRangeAttribute?) Attribute.GetCustomAttribute(vol!, typeof(AllowedRangeAttribute));
            var mVolAttr = (AllowedRangeAttribute?)Attribute.GetCustomAttribute(mVol!, typeof(AllowedRangeAttribute));
            var condition1 = (volAttr!.Min <= tank.Volume) && (tank.Volume <= volAttr.Max);
            var condition2 = (mVolAttr!.Min <= tank.Maxvolume) && (tank.Maxvolume <= mVolAttr.Max);
            return condition1 && condition2;
        }
        static void WriteAllDescriptions(Type t)
        {
            var props = t.GetProperties();
            foreach (var prop in props)
            {
                var desc = (CustomDescriptionAttribute?)Attribute.GetCustomAttribute(prop!, typeof(CustomDescriptionAttribute));
                Console.WriteLine($"Описание для { prop.Name}: {desc!.Description}");
            }
        }
    }
}