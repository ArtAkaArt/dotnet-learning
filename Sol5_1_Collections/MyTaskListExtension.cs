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
        public static async Task<IReadOnlyCollection<T>> RunInParallel<T>(this IEnumerable<Func<Task<T>>> functs, int maxTasks = 4, bool throwException = false)
        {
            ConcurrentBag<T> list = new();
            var semaphore = new SemaphoreSlim(maxTasks);
            var tasks = new List<Task>();
            foreach (var func in functs)
            {
                var task = await Task.Factory.StartNew(async () => {
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
        public static async IAsyncEnumerable<Post> RunInParallelAlt(this IEnumerable<Func<Task<Post>>> functs, int maxTasks = 4, bool throwException = false)
        {
            List<Exception> exList = new();
            var semaphore = new SemaphoreSlim(maxTasks);
            foreach (var func in functs)
            {
                var post = new Post();
                var hadResult = true;
                try
                {
                    post = await Task.Run(async () =>
                    {
                        await semaphore.WaitAsync();
                        try
                        {
                            return await func.Invoke();
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    });

                }
                catch (Exception ex)
                {
                    if (throwException) throw new AggregateException(ex);
                    exList.Add(ex);
                    hadResult = false;
                }
                if (hadResult) yield return post;
            };
            if (exList.Count > 0) throw new AggregateException(exList);// возвращения списка ошибок, если они возникли
        }
    }
}
