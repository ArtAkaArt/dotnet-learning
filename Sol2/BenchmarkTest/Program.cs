using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace Sol2
{
    public class ShiftFinders
    {
        private int[] array;

        public ShiftFinders()
        {
            array = new int[] { 1, 2, 5, 6, 7, 8, 11, 12, 15, 16, 18, 20 };
        }

        [Benchmark]
        public int ShiftAlt() => Solution.Program.GetShiftPositionAlt(array);

        [Benchmark]
        public int ShiftOriginal() => Solution.Program.GetShiftPosition(array);
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<ShiftFinders>(ManualConfig.Create(DefaultConfig.Instance)
                    .WithOptions(ConfigOptions.JoinSummary)
                    .WithOptions(ConfigOptions.DisableOptimizationsValidator)
                    .WithOptions(ConfigOptions.DisableLogFile));
            Console.WriteLine(summary.TotalTime);
        }
    }
}