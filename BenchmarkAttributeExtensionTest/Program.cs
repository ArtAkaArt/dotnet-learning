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
        ConcurrentDictionary<object, PropertyInfo[]> bag = new();
        private readonly PseudoContext context;

        public FilterSpeedTest()
        {
            var tank1 = new CreateTankDTO {Name = "string", Description = "string", Volume = 0, Maxvolume = 0 };
            var tank2 = new TankDTO { Name = "string", Description = "string", Volume = 0, Maxvolume = 0, Id = 0, Unitid = 0 };
            bag.TryAdd(new CreateTankDTO(), tank1.GetType().GetProperties());
            bag.TryAdd(new TankDTO(), tank2.GetType().GetProperties());
            var dictionary = new Dictionary<string, object>();
            dictionary.Add("CreateTank", tank1);
            dictionary.Add("Tank", tank2);
            context = new PseudoContext(dictionary);
            filter = new AttributeFilter(bag);

        }

        [Benchmark]
        public int Filter() => filter.OnActionExecutionAsync(context);

        [Benchmark]
        public int FilterAlt() => filter.OnActionExecutionAltAsync(context);
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