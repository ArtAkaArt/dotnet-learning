using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Sol3.Profiles;
using CustomAttributes;

namespace Test
{
    [MemoryDiagnoser]
    public class FilterSpeedTest
    {
        PseudoContext context1;
        CustomAttributeFilter filter1 = new();
        CacheCustomFilter filter2 = new();

        public FilterSpeedTest()
        {
            var tank1 = new CreateTankDTO { Name = "string", Description = "string", Volume = 0, Maxvolume = 0 };
            var tank2 = new TankDTO { Name = "string", Description = "string", Volume = 0, Maxvolume = 0, Id = 0, Unitid = 0 };
            var unit = new UnitDTO { Name = "string", Description = "string", };
            var unit2 = new CreateUnitDTO { Name = "string", Description = "string", };
            var dictionary = new Dictionary<string, object>();
            //dictionary.Add("CreateTank1", tank1);
            dictionary.Add("asd",new Obj9());
            dictionary.Add("asd2", new Obj8());
            dictionary.Add("asd33", new Obj7());
            dictionary.Add("as2d", new Obj10());
            dictionary.Add("as3d", new Obj9());
            context1 = new PseudoContext(dictionary);

        }

        [Benchmark, WarmupCount(5)]
        public void Filter() => filter1.OnActionExecuting(context1);


        [Benchmark, WarmupCount(5)]
        public void FilterAlt() => filter2.OnActionExecuting(context1);
    }
    public class Program
    {
        public static void Main()
        {
            var summary = BenchmarkRunner.Run<FilterSpeedTest>(ManualConfig.Create(DefaultConfig.Instance)
                    .WithOptions(ConfigOptions.JoinSummary)
                    .WithOptions(ConfigOptions.DisableOptimizationsValidator)
                    .WithOptions(ConfigOptions.DisableLogFile));
            Console.WriteLine(summary.TotalTime);
        }
    }
}