using Xunit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using Sol5_1_Collections;

namespace MyEnumTesting
{
    public class MyEnumerableTesting
    {
		[Theory]
		[MemberData(nameof(ListCompTest))]
		public async void Lists_should_be_equal(List<Func<CancellationToken, Task<Post>>> funcs, bool result)
		{
			Assert.Equal(expected: result, actual: await Program.CompareLists3(funcs));
		}

		public static IEnumerable<object[]> ListCompTest()
		{
			yield return new object[] { ListMakers.MakeList1(), true };
			yield return new object[] { ListMakers.MakeList2(), true };
			yield return new object[] { ListMakers.MakeList3(), true };
			yield return new object[] { ListMakers.MakeList4(), true };
			yield return new object[] { ListMakers.MakeList5(), true };
			yield return new object[] { ListMakers.MakeList6(), true };
			yield return new object[] { ListMakers.MakeList7(), true };
			yield return new object[] { ListMakers.MakeList8(), true };
		}
	}
}