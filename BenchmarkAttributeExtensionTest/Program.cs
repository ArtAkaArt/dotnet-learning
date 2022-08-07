using Attributes;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System.Collections.Concurrent;
using Sol3;
using Sol3.Profiles;
using System.Reflection;

namespace Test
{
    public class FilterSpeedTest
    {

        private readonly AttributeFilter filter;
        ConcurrentDictionary<Type, PropertyInfo[]> bag = new();
        private readonly PseudoContext context;

        public FilterSpeedTest()
        {
            var tank1 = new CreateTankDTO {Name = "string", Description = "string", Volume = 0, Maxvolume = 0 };
            var tank2 = new TankDTO { Name = "string", Description = "string", Volume = 0, Maxvolume = 0, Id = 0, Unitid = 0 };
            var unit = new UnitDTO { Name = "string", Description = "string", };
            var unit2 = new CreateUnitDTO { Name = "string", Description = "string", };
            bag.TryAdd(new CreateTankDTO().GetType(), tank1.GetType().GetProperties());
            bag.TryAdd(new TankDTO().GetType(), tank2.GetType().GetProperties());
            var dictionary = new Dictionary<string, object>();
            dictionary.Add("CreateTank1", tank1);
            dictionary.Add("Tank32", unit);
            dictionary.Add("Tank42", unit);
            dictionary.Add("Tank52", unit);
            dictionary.Add("Tank62", unit);
            dictionary.Add("Tank72", unit);
            dictionary.Add("Tank3", unit2);
            context = new PseudoContext(dictionary);
            filter = new AttributeFilter(bag);

        }

        [Benchmark]
        public void Filter() => filter.OnActionExecutionAsync(context);

        [Benchmark]
        public void FilterAlt() => filter.OnActionExecutionAltAsync(context);
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