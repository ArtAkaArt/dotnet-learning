using System.Collections.Concurrent;
using System.Text;

namespace Sol5_1_Collections
{
    public static class MyTaskListExtention
    {
        /// <summary>
        /// Асинхронный метод возвращающий коллекцию объектов, хранящихся в IReadOnlyCollection. Может принимать параметры int maxTasks и bool throwException, имеющие
        /// значения по умолчанию 4 и false.</summary>
        /// <typeparam name="T"></typeparam> Тип возвращаемого объекта.
        /// <param name="maxTasks"> Определяет количество одновременно исполняемых потоков для получения объектов</param>
        /// <param name="throwException">Определяет будет ли выброшена первая ошибка при получении объектов и остановлен метод.
        /// В режиме false будут переданы все объекты, которые удалось получить и все полученные ошибки через AggregateException</param>
        /// <returns></returns>
        /// <exception cref="AggregateException"></exception>
        public static async Task<IReadOnlyCollection<T>> RunInParallel<T>(this IEnumerable<Func<Task<T>>> functs, CancellationTokenSource tokenSource, int maxTasks = 4, bool throwException = false)
        {
            var token = tokenSource.Token;
            ConcurrentBag<T> list = new();
            var semaphore = new SemaphoreSlim(maxTasks);
            var tasks = new List<Task>();
            foreach (var func in functs)
            {
                var task = Task.Run(async () => {
                    await semaphore.WaitAsync();
                    try
                    {
                        list.Add(await func.Invoke());
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });
                tasks.Add(task);
            }
            Console.WriteLine(tasks.Count(t => t != null) + " Task count");
            try
            {
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                if (throwException)
                {
                    var exceptions = tasks.Where(t => t.Exception != null)
                                          .Select(t => t.Exception);
                    throw new AggregateException(exceptions);
                }
            }
            Console.WriteLine("End in RunInParallel");
            return list;
        }
        public static async IAsyncEnumerable<T> RunInParallelAlt<T>(this IEnumerable<Func<Task<T>>> functs, CancellationTokenSource tokenSource, int maxTasks = 4, bool throwException = false)
        {
            var token = tokenSource.Token;
            List<Exception> exList = new();
            var semaphore = new SemaphoreSlim(maxTasks);
            var tasks = new List<Task<T>>();
            foreach (var func in functs)
            {
                var task = Task.Run(async () => {
                    await semaphore.WaitAsync();
                    try
                    {
                        return await func.Invoke();
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }, token);
                tasks.Add(task);
            }
            await Task.WhenAny(tasks);
            foreach (var task in tasks) //я не знаю почему оно работает)
            {
                if (task.Exception != null && throwException) { tokenSource.Cancel(); throw new AggregateException(task.Exception); }
                else if (task.Exception != null) exList.Add(task.Exception);
                else yield return task.Result;
            }
            if (exList.Count > 0) throw new AggregateException(exList);// возвращения списка ошибок, если они возникли
        }
    }
}
