using Xunit;
using System.Linq;
using Solution;
using System.Collections.Generic;

namespace TestProject
{
    public class NumericShiftDetectorTests
    {
		[Theory]
		[MemberData(nameof(ArraysAndShifts))]
		public void Should_detect_shifts(int[] sample, int result) 
		{
			Assert.Equal(expected: result, actual: Program.GetShiftPositionAlt(sample));
		}

		public static IEnumerable<object[]> ArraysAndShifts()
		{
			yield return new object[] { new int[] { 15, 16, 18, 20, 1, 2, 5, 6, 7, 8, 11, 12 }, 4};
			yield return new object[] { new int[] { 5, 6, 7, 8, 11, 12, 15, 16, 18, 1, 2, 3 },9 };
			yield return new object[] { new int[] { 5, 6, 7, 8, 11, 12, 15, 16, 18, 20, 1, 2 }, 10 };
			yield return new object[] { new int[] { 11, 1, 2, 5 }, 1 };
			yield return new object[] { new int[] { 5, 6, 1, 2 }, 2 };
			yield return new object[] { new int[] { 7, 11, 1, 2, 5 }, 2 };
			yield return new object[] { new int[] { 1, 2, 5, 6, 7, 8, 11, 12, 15, 16, 18, 20 }, 0 };
			yield return new object[] { new int[] { 5 }, 0 };
			var part1 = Enumerable.Range(15_000, 100_000);
			var part2 = Enumerable.Range(0, 14_995);
			var arr = part1.Concat(part2).ToArray();
			yield return new object[] { arr, 100_000 };
		}
	}
}