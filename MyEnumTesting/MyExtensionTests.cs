﻿using Xunit;
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
    public class MyExtensionTests
    {
        [Fact]
        public async void Run_FiftyFuncsAsAsyncEnumerable_FiftyResultExpected()
        {
            var funcs = Enumerable.Range(1, 50)
                        .Select(i => (Func<Task<int>>)(async () => await GetResult(i)))
                        .AsAsyncEumerable();

            var (results, exceptions) = await GetResultFromRunAwaitForeach(funcs);

            Assert.NotNull(results);
            results.Sort();
            Assert.Null(exceptions);
            Assert.Equal(actual: results, expected: Enumerable.Range(1, 50).ToList());
        }

        [Fact]
        public async void Run_FiftyFuncsAsAsyncEnumerable_OneExceptionExpected()
        {
            var funcs = Enumerable.Range(2, 49)
                        .Select(i => (Func<Task<int>>)(async () => await GetResult(i)))
                        .ToList();
            funcs.Insert(0, async () => await GetException(1));

            var artificialResult = Enumerable.Range(2, 49).ToList();

            var (results, exceptions) = await GetResultFromRunAwaitForeach(funcs.AsAsyncEumerable());

            Assert.NotNull(results);
            results.Sort();
            Assert.Equal(actual: results, expected: artificialResult);
            Assert.Contains("Ошибка при получении поста номер: 1", exceptions!.InnerExceptions.FirstOrDefault()!.Message);
            Assert.True(exceptions!.InnerExceptions.Count == 1);
        }

        [Fact]
        public async void Run_FiftyFuncsAsAsyncEnumerable_TenExceptionsExpected()
        {
            var funcs = Enumerable.Range(1, 40)
                        .Select(i => (Func<Task<int>>)(async () => await GetResult(i)))
                        .ToList();
            var initialList = Enumerable.Range(1, 50);
            funcs.AddRange(Enumerable.Range(41, 10).
                Select(i => (Func<Task<int>>)(async () => await GetException(i))));
            var artificialResult = initialList.Where(i => i < 41).ToList();
            var arificialAggEx = ConstructAggEx(initialList.Where(i => i > 40).ToList());

            var (results, exceptions) = await GetResultFromRunAwaitForeach(funcs.AsAsyncEumerable());

            Assert.NotNull(results);
            results.Sort();
            Assert.Equal(actual: results, expected: artificialResult);
            Assert.True(CompareAggregateEx(exceptions!, arificialAggEx));
        }

        [Fact]
        public async void Run_FiftyFuncsAsAsyncEnumerable_FiftyExceptionsExpected()
        {
            var funcs = Enumerable.Range(1, 50)
                        .Select(i => (Func< Task<int>>)(async () => await GetException(i)));
            var arificialAggEx = ConstructAggEx(Enumerable.Range(1, 50).ToList());

            var (results, exceptions) = await GetResultFromRunAwaitForeach(funcs.AsAsyncEumerable());

            Assert.Null(results);
            Assert.True(CompareAggregateEx(exceptions!, arificialAggEx));
        }

        [Fact]
        public async void Run_OneFuncAsAsyncEnumerable_OneResultExpected()
        {
            var funcs = Enumerable.Range(1, 1)
                        .Select(i => (Func<Task<int>>)(async () => await GetResult(i)));

            var (results, exceptions) = await GetResultFromRunAwaitForeach(funcs.AsAsyncEumerable());

            Assert.Equal(actual: results, expected: Enumerable.Range(1, 1).ToList());
            Assert.Null(exceptions);
        }

        [Fact]
        public async void Run_OneFuncAsAsyncEnumerable_OneExceptionExpected()
        {
            var funcs = Enumerable.Range(1, 1)
                        .Select(i => (Func<Task<int>>)(async () => await GetException(i)));

            var (results, exceptions) = await GetResultFromRunAwaitForeach(funcs.AsAsyncEumerable());

            Assert.Null(results);
            Assert.Contains("Ошибка при получении поста номер: 1", exceptions!.InnerExceptions.First().Message);
            Assert.True(exceptions.InnerExceptions.Count == 1);
        }

        [Fact]
        public async void Run_EmptyListAsAsyncEnumerable_NoResultExpected()
        {
            var funcs = new List<Func<Task<int>>>();

            var (results, exceptions) = await GetResultFromRunAwaitForeach(funcs.AsAsyncEumerable());

            Assert.Null(results);
            Assert.Null(exceptions);
        }

        [Fact]
        public async void Run_TenFuncAsAsyncEnumerable_RuntimeLongerOrEqualThan10sExpected()
        {
            var funcs = Enumerable.Range(1, 10)
                        .Select(i => (Func<Task<int>>)(async () => await GetResultIn1Sec(i)));
            var stopWatch = Stopwatch.StartNew();

            var (results, exceptions) = await GetResultFromRunAwaitForeach(funcs.AsAsyncEumerable(1));
            var time = stopWatch.Elapsed.Seconds;

            Assert.NotNull(results);
            results.Sort();
            Assert.Equal(actual: results, expected: Enumerable.Range(1, 10).ToList());
            Assert.Null(exceptions);
            Assert.True(time >= 10);
        }
        [Fact]
        public async void Run_TenFuncAsAsyncEnumerable_RuntimeLongerOrEqualThan5sExpected()
        {
            var funcs = Enumerable.Range(1, 10)
                        .Select(i => (Func< Task<int>>)(async () => await GetResultIn1Sec(i)));
            var stopWatch = Stopwatch.StartNew();

            var (results, exceptions) = await GetResultFromRunAwaitForeach(funcs.AsAsyncEumerable(2));
            var time = stopWatch.Elapsed.Seconds;

            Assert.NotNull(results);
            results.Sort();
            Assert.Equal(actual: results, expected: Enumerable.Range(1, 10).ToList());
            Assert.Null(exceptions);
            Assert.True(time >= 5);
        }
        [Fact]
        public async void Run_TwentyFiveFuncsAsAsyncEnumerable_PredictableLastValueExpected()
        {
            var funcs = Enumerable.Range(1, 25)
                        .Select(i => (Func<Task<int>>)(async () => await GetResult(i)))
                        .ToList();
            funcs.Insert(0, async () => await GetResultIn10Sec(1));

            var (results, exceptions) = await GetResultFromRunAwaitForeach(funcs.AsAsyncEumerable());

            Assert.Equal(actual: results!.Last(), expected: 1010101);
        }
        [Fact]
        public async void Run_TenFuncAsAsyncEnumerable_OneExceptionAndLessThanEightResultsExpected()
        {
            var funcs = Enumerable.Range(1, 10)
                        .Select(i => (Func<Task<int>>)(async () => await GetResult(i)))
                        .ToList();
            funcs.Insert(1, (async () => await GetException(1)));

            var (results, exceptions) = await GetResultFromRunAwaitForeach(funcs.AsAsyncEumerable(4, ErrorsHandleMode.EndAtFirstError));

            Assert.True(results!.Count < 8);
            Assert.True(exceptions!.InnerExceptions.Count == 1);
            Assert.Contains("Ошибка при получении поста номер: 1", exceptions!.InnerExceptions.First().Message);
        }

        [Fact]
        public async void Run_FiftyFuncsAsAsyncEnumerable_FortyNineResultsAndNoExceptionsExpected()
        {
            var funcs = Enumerable.Range(2, 49)
                        .Select(i => (Func<Task<int>>)(async () => await GetResult(i)))
                        .ToList();
            funcs.Insert(1, (async () => await GetException(1)));

            var (results, exceptions) = await GetResultFromRunAwaitForeach(funcs.AsAsyncEumerable(4, ErrorsHandleMode.IgnoreErrors));

            Assert.NotNull(results);
            results.Sort();
            Assert.Equal(actual: results, expected: Enumerable.Range(2, 49).ToList());
            Assert.Null(exceptions);
        }
        [Fact]
        public async void Run_FiftyFuncsAsAsyncEnumerable_ReturnNoResultsExpected()
        {
            var funcs = Enumerable.Range(1, 50)
                        .Select(i => (Func<Task<int>>)(async () => await GetException(i)));

            var (results, exceptions) = await GetResultFromRunAwaitForeach(funcs.AsAsyncEumerable(4, ErrorsHandleMode.IgnoreErrors));

            Assert.Null(results);
            Assert.Null(exceptions);
        }
    }
}
