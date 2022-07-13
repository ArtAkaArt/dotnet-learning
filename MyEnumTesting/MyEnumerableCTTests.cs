using Xunit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using Sol5_1_Collections;
using System.Linq;
using System.Diagnostics;
using static MyEnumTesting.TestedMethods;
using static MyEnumTesting.HelpMethods;

namespace MyEnumTesting
{
    public class MyEnumerableCTTests
    {
        [Fact]
        public async void Run_MyEnumerable_FiftyValidTasks_ResultExpected_CT()
        {
            var funcs = Enumerable.Range(1, 50).
                Select(i => (Func<CancellationToken, Task<int>>)(async (CancellationToken cs) => await GetResultCT(i, cs)));

            var myEnum = new MyAsyncEnumerable<int>(funcs);

            var (results, exceptions) = await GetResultFromRunAwaitForeach(myEnum);
            results!.Sort();
            Assert.Null(exceptions);
            Assert.Equal(actual: results, expected: Enumerable.Range(1, 50).ToList());
        }

        [Fact]
        public async void Run_MyEnumerable_OneException_ResultExpected_CT()
        {
            var funcs = Enumerable.Range(2, 49).
                Select(i => (Func<CancellationToken, Task<int>>)(async (CancellationToken cs) => await GetResultCT(i, cs))).ToList();
            funcs.Insert(0, async (CancellationToken cs) => await GetExceptionCT(1, cs));
            var myEnum = new MyAsyncEnumerable<int>(funcs);
            var artificialResult = Enumerable.Range(2, 49).ToList();

            var (results, exceptions) = await GetResultFromRunAwaitForeach(myEnum);
            results!.Sort();

            Assert.Equal(actual: results, expected: artificialResult);
            Assert.Contains("Ошибка при получении поста номер: 1", exceptions!.InnerExceptions.FirstOrDefault()!.Message);
            Assert.True(exceptions!.InnerExceptions.Count == 1);
        }

        [Fact]
        public async void Run_MyEnumerable_MultipleExceptions_ResultExpected_CT()
        {
            var funcs = Enumerable.Range(1, 40).
                Select(i => (Func<CancellationToken, Task<int>>)(async (CancellationToken cs) => await GetResultCT(i, cs))).ToList();
            funcs.AddRange(Enumerable.Range(41, 10).
                Select(i => (Func<CancellationToken, Task<int>>)(async (CancellationToken cs) => await GetExceptionCT(i, cs))));
            var myEnum = new MyAsyncEnumerable<int>(funcs);
            var initialList = Enumerable.Range(1, 50);
            var artificialResult = initialList.Where(i => i < 41).ToList();
            var arificialAggEx = ConstructAggEx(initialList.Where(i => i > 40).ToList());

            var (results, exceptions) = await GetResultFromRunAwaitForeach(myEnum);
            results!.Sort();

            Assert.Equal(actual: results, expected: artificialResult);
            Assert.True(CompareAggregateEx(exceptions!, arificialAggEx));
        }

        [Fact]
        public async void Run_MyEnumerable_AllExceptions_ResultExpected_CT()
        {
            var funcs = Enumerable.Range(1, 50).
                Select(i => (Func<CancellationToken, Task<int>>)(async (CancellationToken cs) => await GetExceptionCT(i, cs)));
            var myEnum = new MyAsyncEnumerable<int>(funcs);
            var arificialAggEx = ConstructAggEx(Enumerable.Range(1, 50).ToList());

            var (results, exceptions) = await GetResultFromRunAwaitForeach(myEnum);

            Assert.Null(results);
            Assert.True(CompareAggregateEx(exceptions!, arificialAggEx));
        }

        [Fact]
        public async void Run_MyEnumerable_OneValid_ResultExpected_CT()
        {
            var funcs = Enumerable.Range(1, 1).
                Select(i => (Func<CancellationToken, Task<int>>)(async (CancellationToken cs) => await GetResultCT(i, cs)));
            var myEnum = new MyAsyncEnumerable<int>(funcs);

            var (results, exceptions) = await GetResultFromRunAwaitForeach(myEnum);

            Assert.Equal(actual: results, expected: Enumerable.Range(1, 1).ToList());
            Assert.Null(exceptions);
        }

        [Fact]
        public async void Run_MyEnumerable_OnlyOneException_ResultExpected_CT()
        {
            var funcs = Enumerable.Range(1, 1).
                Select(i => (Func<CancellationToken, Task<int>>)(async (CancellationToken cs) => await GetExceptionCT(i, cs)));
            var myEnum = new MyAsyncEnumerable<int>(funcs);

            var (results, exceptions) = await GetResultFromRunAwaitForeach(myEnum);

            Assert.Null(results);
            Assert.Contains("Ошибка при получении поста номер: 1", exceptions!.InnerExceptions.First().Message);
            Assert.True(exceptions.InnerExceptions.Count == 1);
        }

        [Fact]
        public async void Run_MyEnumerable_EmptyList_ResultExpected_CT()
        {
            var funcs = new List<Func<CancellationToken, Task<int>>>();
            var myEnum = new MyAsyncEnumerable<int>(funcs);

            var (results, exceptions) = await GetResultFromRunAwaitForeach(myEnum);

            Assert.Null(results);
            Assert.Null(exceptions);
        }

        //тестирование семафора
        [Fact]
        public async void Run_MyEnumerable_RuntimeLongerOrEqualThan10s_ResultExpected_CT()
        {
            var funcs = Enumerable.Range(1, 10).
                Select(i => (Func<CancellationToken, Task<int>>)(async (CancellationToken cs) => await GetResultIn1SecCT(i, cs)));
            var myEnum = new MyAsyncEnumerable<int>(funcs, 1);
            var stopWatch = Stopwatch.StartNew();

            var (results, exceptions) = await GetResultFromRunAwaitForeach(myEnum);
            var time = stopWatch.Elapsed.Seconds;
            results!.Sort();

            Assert.Equal(actual: results, expected: Enumerable.Range(1, 10).ToList());
            Assert.Null(exceptions);
            Assert.True(time >= 10);
        }
        [Fact]
        public async void Run_MyEnumerable_RuntimeLongerOrEqualThan5s_ResultExpected_CT()
        {
            var funcs = Enumerable.Range(1, 10).
                Select(i => (Func<CancellationToken, Task<int>>)(async (CancellationToken cs) => await GetResultIn1SecCT(i, cs)));
            var myEnum = new MyAsyncEnumerable<int>(funcs, 2);
            var stopWatch = Stopwatch.StartNew();

            var (results, exceptions) = await GetResultFromRunAwaitForeach(myEnum);
            var time = stopWatch.Elapsed.Seconds;
            results!.Sort();

            Assert.Equal(actual: results, expected: Enumerable.Range(1, 10).ToList());
            Assert.Null(exceptions);
            Assert.True(time >= 5);
        }
        [Fact]
        public async void Run_MyEnumerable_PredictableLastValue_Expected_CT()
        {
            var funcs = Enumerable.Range(1, 25)
            .Select(i => (Func<CancellationToken, Task<int>>)(async (CancellationToken cs) => await GetResultCT(i, cs))).ToList();
            funcs.Insert(0, async (CancellationToken cs) => await GetResultIn10SecCT(1, cs));
            var myEnum = new MyAsyncEnumerable<int>(funcs);

            var (results, exceptions) = await GetResultFromRunAwaitForeach(myEnum);

            Assert.Equal(actual: results!.Last(), expected: 1010101);
        }
        [Fact]
        public async void Run_MyEnumerable_OneExceptionAndLessThanEight_ResultsExpected_CT()
        {
            var funcs = Enumerable.Range(1, 10).
                Select(i => (Func<CancellationToken, Task<int>>)(async (CancellationToken cs) => await GetResultCT(i, cs))).ToList();
            funcs.Insert(1, (async (CancellationToken cs) => await GetExceptionCT(1, cs)));
            var myEnum = new MyAsyncEnumerable<int>(funcs, 4, ErrorsHandleMode.EndAtFirstError);

            var (results, exceptions) = await GetResultFromRunAwaitForeach(myEnum);

            Assert.True(results!.Count < 8);
            Assert.True(exceptions!.InnerExceptions.Count == 1);
            Assert.Contains("Ошибка при получении поста номер: 1", exceptions!.InnerExceptions.First().Message);
        }

        [Fact]
        public async void Run_MyEnumerable_IgnoreThrowedExceptions_Expected_CT()
        {
            var funcs = Enumerable.Range(2, 49).
                Select(i => (Func<CancellationToken, Task<int>>)(async (CancellationToken cs) => await GetResultCT(i, cs))).ToList();
            funcs.Insert(1, (async (CancellationToken cs) => await GetExceptionCT(1, cs)));
            var myEnum = new MyAsyncEnumerable<int>(funcs, 4, ErrorsHandleMode.IgnoreErrors);

            var (results, exceptions) = await GetResultFromRunAwaitForeach(myEnum);
            results!.Sort();

            Assert.Equal(actual: results, expected: Enumerable.Range(2, 49).ToList());
            Assert.Null(exceptions);
        }
        [Fact]
        public async void Run_MyEnumerable_ReturnNoResult_Expected_CT()
        {
            var funcs = Enumerable.Range(1, 50).
                Select(i => (Func<CancellationToken, Task<int>>)(async (CancellationToken cs) => await GetExceptionCT(i, cs)));
            var myEnum = new MyAsyncEnumerable<int>(funcs, 4, ErrorsHandleMode.IgnoreErrors);

            var (results, exceptions) = await GetResultFromRunAwaitForeach(myEnum);

            Assert.Null(results);
            Assert.Null(exceptions);
        }
    }
}
