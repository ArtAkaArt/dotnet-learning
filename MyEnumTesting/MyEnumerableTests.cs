using Xunit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using Sol5_1_Collections;
using System.Linq;
using System.Diagnostics;

namespace MyEnumTesting
{
    public class MyEnumerableTests
    {
        [Fact]
        public async void Run_MyEnumerable_FiftyValidTasks_ResultExpected()
        {
            var funcs = Enumerable.Range(1, 50)
            .Select(i => (Func<CancellationToken, Task<int>>)(async (CancellationToken cs) => await GetResultWithoutExceptions(i, cs)));

            var myEnum = new MyAsyncEnumerable<int>(funcs);

            var (results, exceptions) = await GetResult_From_RunAwaitForeach(myEnum);
            results.Sort();
            Assert.Equal(actual: exceptions.InnerExceptions.Count, expected: 0);
            Assert.Equal(actual: results, expected: Enumerable.Range(1, 50).ToList());
        }

        [Fact]
        public async void Run_MyEnumerable_OneException_ResultExpected()
        {
            var funcs = Enumerable.Range(1, 50)
            .Select(i => (Func<CancellationToken, Task<int>>)(async (CancellationToken cs) => await GetResultWithOneException(i, cs)));

            var myEnum = new MyAsyncEnumerable<int>(funcs);
            var artificialResult = Enumerable.Range(2, 49).ToList();
            var arificialAggEx = new AggregateException(new Exception("Ошибка при получении поста номер: 1"));

            var (results, exceptions) = await GetResult_From_RunAwaitForeach(myEnum);
            results.Sort();

            Assert.Equal(actual: results, expected: artificialResult);
            Assert.True(CompareAggregateEx(exceptions, arificialAggEx));
        }

        [Fact]
        public async void Run_MyEnumerable_MultipleExceptions_ResultExpected()
        {
            var funcs = Enumerable.Range(1, 50)
            .Select(i => (Func<CancellationToken, Task<int>>)(async (CancellationToken cs) => await GetResultWithMultipleExceptions(i, cs)));
            var myEnum = new MyAsyncEnumerable<int>(funcs);
            var initialList = Enumerable.Range(1, 50).ToList();
            var artificialResult = initialList.Where(i => i % 5 != 0).ToList();
            var arificialAggEx = ConstructAggEx(initialList.Where(i => i % 5 == 0).ToList());

            var (results, exceptions) = await GetResult_From_RunAwaitForeach(myEnum);
            results.Sort();

            Assert.Equal(actual: results, expected: artificialResult);
            Assert.True(CompareAggregateEx(exceptions, arificialAggEx));
        }

        [Fact]
        public async void Run_MyEnumerable_AllExceptions_ResultExpected()
        {
            var funcs = Enumerable.Range(1, 50)
            .Select(i => (Func<CancellationToken, Task<int>>)(async (CancellationToken cs) => await GetAllExceptions(i, cs)));
            var myEnum = new MyAsyncEnumerable<int>(funcs);
            var arificialAggEx = ConstructAggEx(Enumerable.Range(1, 50).ToList());

            var (results, exceptions) = await GetResult_From_RunAwaitForeach(myEnum);

            Assert.Equal(actual: results.Count, expected: 0);
            Assert.True(CompareAggregateEx(exceptions, arificialAggEx));
        }

        [Fact]
        public async void Run_MyEnumerable_OneValid_ResultExpected()
        {
            var funcs = Enumerable.Range(1, 1)
            .Select(i => (Func<CancellationToken, Task<int>>)(async (CancellationToken cs) => await GetResultWithoutExceptions(i, cs)));
            var myEnum = new MyAsyncEnumerable<int>(funcs);

            var (results, exceptions) = await GetResult_From_RunAwaitForeach(myEnum);

            Assert.Equal(actual: results, expected: Enumerable.Range(1, 1).ToList());
            Assert.Equal(actual: exceptions.InnerExceptions.Count, expected: 0);
        }

        [Fact]
        public async void Run_MyEnumerable_OnlyOneException_ResultExpected()
        {
            var funcs = Enumerable.Range(1, 1)
            .Select(i => (Func<CancellationToken, Task<int>>)(async (CancellationToken cs) => await GetAllExceptions(i, cs)));
            var myEnum = new MyAsyncEnumerable<int>(funcs);
            var arificialAggEx = ConstructAggEx(Enumerable.Range(1, 1).ToList());

            var (results, exceptions) = await GetResult_From_RunAwaitForeach(myEnum);

            Assert.Equal(actual: results.Count, expected: 0);
            Assert.True(CompareAggregateEx(exceptions, arificialAggEx));
        }

        [Fact]
        public async void Run_MyEnumerable_EmptyList_ResultExpected()
        {
            var funcs = new List<Func<CancellationToken, Task<int>>>();
            var myEnum = new MyAsyncEnumerable<int>(funcs);

            var (results, exceptions) = await GetResult_From_RunAwaitForeach(myEnum);

            Assert.Equal(actual: results.Count, expected: 0);
            Assert.Equal(actual: exceptions.InnerExceptions.Count, expected: 0);
        }

        //тестирование семафора
        [Fact]
        public async void Run_MyEnumerable_RuntimeLongerOrEqualThan10s_ResultExpected()
        {
            var funcs = Enumerable.Range(1, 10)
            .Select(i => (Func<CancellationToken, Task<int>>)(async (CancellationToken cs) => await GetResultIn1Sec(i, cs)));
            var myEnum = new MyAsyncEnumerable<int>(funcs, 1);
            var stopWatch = Stopwatch.StartNew();

            var (results, exceptions) = await GetResult_From_RunAwaitForeach(myEnum);
            var time = stopWatch.Elapsed.Seconds;
            results.Sort();

            Assert.Equal(actual: results, expected: Enumerable.Range(1, 10).ToList());
            Assert.Equal(actual: exceptions.InnerExceptions.Count, expected: 0);
            Assert.True(time >= 10);
        }

        [Fact]
        public async void Run_MyEnumerable_OneExceptionAndLessThanEight_ResultsExpected()
        {
            var funcs = Enumerable.Range(1, 10)
            .Select(i => (Func<CancellationToken, Task<int>>)(async (CancellationToken cs) => await GetResultWithOneException(i, cs)));
            var myEnum = new MyAsyncEnumerable<int>(funcs, 4, ErrorsHandleMode.EndAtFirstError);

            var (results, exceptions) = await GetResult_From_RunAwaitForeach(myEnum);

            Assert.True(results.Count < 8);
            Assert.Equal(actual: exceptions.InnerExceptions.Count, expected: 1);
        }

        [Fact]
        public async void Run_MyEnumerable_IgnoreThrowedExceptions_Expected()
        {
            var funcs = Enumerable.Range(1, 50)
            .Select(i => (Func<CancellationToken, Task<int>>)(async (CancellationToken cs) => await GetResultWithOneException(i, cs)));
            var myEnum = new MyAsyncEnumerable<int>(funcs, 4, ErrorsHandleMode.IgnoreErrors);

            var (results, exceptions) = await GetResult_From_RunAwaitForeach(myEnum);
            results.Sort();

            Assert.Equal(actual: results, expected: Enumerable.Range(2, 49).ToList());
            Assert.Equal(actual: exceptions.InnerExceptions.Count, expected: 0);
        }
        [Fact]
        public async void Run_MyEnumerable_PredictableLastValue_Expected()
        {
            var funcs = Enumerable.Range(1, 25)
            .Select(i => (Func<CancellationToken, Task<int>>)(async (CancellationToken cs) => await GetResultWithoutExceptions(i, cs))).ToList();
            funcs.Insert(0, async (CancellationToken cs) => await GetResultIn10Sec(cs));
            var myEnum = new MyAsyncEnumerable<int>(funcs, 4, ErrorsHandleMode.IgnoreErrors);

            var (results, exceptions) = await GetResult_From_RunAwaitForeach(myEnum);

            Assert.Equal(actual: results.Last(), expected: 1010101);
        }

        private bool CompareAggregateEx(AggregateException aggEx, AggregateException imitation)
        {
            var isExsEqual = true;
            if (imitation.InnerExceptions.Count != aggEx.InnerExceptions.Count) return false;
            foreach (var inEx1 in imitation.InnerExceptions)
            {
                var inEx2 = aggEx.InnerExceptions.FirstOrDefault(i => i.Message.Contains(inEx1.Message));
                isExsEqual = isExsEqual && inEx2 != null;
            }
            return isExsEqual;
        }
        private bool CompareResultsLists(List<int> list1, List<int> list2, bool isSortNeeded = true)
        {
            if (isSortNeeded)
                list1.Sort();
            var isListsEqual = true;
            if (list1.Count != list2.Count) return false;
            foreach (var item2 in list2)
            {
                var item1 = list2.FirstOrDefault(x => x == item2);
                isListsEqual = isListsEqual && item1 == item2;
            }
            return isListsEqual;
        }
        private AggregateException ConstructAggEx(List<int> numbers)
        {
            var list = new List<Exception>();
            foreach (var number in numbers)
            {
                list.Add(new Exception("Ошибка при получении поста номер: " + number));
            }
            return new AggregateException(list);
        }
        private async Task<(List<int> results, AggregateException exceptions)> GetResult_From_RunAwaitForeach(MyAsyncEnumerable<int> enumerable)
        {
            var results = new List<int>();
            var ex = new AggregateException();
            try
            {
                await foreach (var res in enumerable)
                {
                    results.Add(res);
                }
            }
            catch (AggregateException ex2)
            {
                ex = ex2;
            }
            return (results, ex);
        }
        private async Task<int> GetResultIn1Sec(int number, CancellationToken ct)
        {
            if (ct.IsCancellationRequested) ct.ThrowIfCancellationRequested();
            await Task.Delay(1000, ct);
            return number;
        }
        private async Task<int> GetResultWithoutExceptions(int number, CancellationToken ct)
        {
            await Task.Delay(100, ct);
            return number;
        }
        private async Task<int> GetResultIn10Sec(CancellationToken ct)
        {
            await Task.Delay(10000, ct);
            return 1010101;
        }
        private async Task<int> GetResultWithOneException(int number, CancellationToken ct)
        {
            if (ct.IsCancellationRequested) ct.ThrowIfCancellationRequested();
            await Task.Delay(100, ct);
            if (number == 1) throw new Exception("Ошибка при получении поста номер: " + number);
            return number;
        }
        private async Task<int> GetResultWithMultipleExceptions(int number, CancellationToken ct)
        {
            if (ct.IsCancellationRequested) ct.ThrowIfCancellationRequested();
            await Task.Delay(100, ct);
            if (number % 5 == 0) throw new Exception("Ошибка при получении поста номер: " + number);
            return number;
        }
        private async Task<int> GetAllExceptions(int number, CancellationToken ct)
        {
            if (ct.IsCancellationRequested) ct.ThrowIfCancellationRequested();
            await Task.Delay(100, ct);
            throw new Exception("Ошибка при получении поста номер: " + number);
        }
    }
}
