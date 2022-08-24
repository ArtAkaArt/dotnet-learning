using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Sol3.Profiles;
using CustomAttributes;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Moq;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Test
{
    [MemoryDiagnoser]
    public class FilterSpeedTest
    {
        PseudoContext context1;
        CustomAttributeFilter filter1 = new();
        CacheCustomFilter filter2 = new();
        ActionExecutingContext actExecutingContext;
        public FilterSpeedTest()
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add("asd",new Obj9());
            dictionary.Add("asd2", new Obj8());
            dictionary.Add("asd33", new Obj7());
            dictionary.Add("as2d", new Obj10());
            dictionary.Add("as3d", new Obj9());

            var actContext = new ActionContext(
               Mock.Of<HttpContext>(),
               Mock.Of<RouteData>(),
               Mock.Of<ActionDescriptor>(),
               Mock.Of<ModelStateDictionary>()
           );
            actExecutingContext = new ActionExecutingContext(
                actContext,
                new List<IFilterMetadata>(),
                dictionary,
                Mock.Of<Controller>()
            );
        }

        [Benchmark, WarmupCount(5)]
        public void Filter() => filter1.OnActionExecuting(actExecutingContext);


        [Benchmark, WarmupCount(5)]
        public void FilterAlt() => filter2.OnActionExecuting(actExecutingContext);
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