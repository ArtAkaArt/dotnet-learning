using System.Collections.Concurrent;

namespace Sol5_1_Collections
{
    public static class MyTaskListExtention
    {
        /// <summary>
        /// Метод асинхронно запускает переданный набор функций Func<Task<T>>, и возвращает результаты их выполнения T в виде набора объектов IReadOnlyCollection<T>.
        /// Метод так же ограничивает одновременное количество исполняемых Task, по умолчанию - 4.
        /// </summary>
        /// <typeparam name="T"></typeparam> Тип возвращаемого объекта.
        /// <param name="maxTasks"> Определяет количество одновременно исполняемых потоков для получения объектов</param>
        /// <param name="throwException">Опциональный параметр, определяет будет ли выброшена первая ошибка при исполнении Task, остановлен метод и вызвана остановка оставшихся Task
        /// или метод вернет все полученные объекты вместе со всеми возникшими ошибками AggregateException. По умолчанию ошибка выброшена не будет.</param>
        /// <param name="tokenSource"> CancellationTokenSource на основе, которого были созданы CancellationToken'ы в переданных Task<T></param>
        /// <returns></returns>
        /// <exception cref="AggregateException"></exception>
        public static async Task<IReadOnlyCollection<T>> RunInParallel<T>(this IEnumerable<Func<CancellationToken, Task<T>>> functs, int maxTasks = 4, bool throwException = false)
        {
            var tokenSrc = new CancellationTokenSource();
            var token = tokenSrc.Token;
            ConcurrentBag<T> list = new();
            var semaphore = new SemaphoreSlim(maxTasks);
            var tasks = new List<Task>();
            foreach (var func in functs)
            {
                var task = Task.Run(async () => {
                    await semaphore.WaitAsync(token);
                    try
                    {
                        list.Add(await func.Invoke(token));
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }, token);
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
                    tokenSrc.Cancel();
                    throw new AggregateException(exceptions);
                }
            }
            Console.WriteLine("End in RunInParallel");
            return list;
        }
        /// <summary>
        /// Метод асинхронно запускает переданный набор функций Func<Task<T>>, и возвращает результаты их выполнения T по мере их получения.
        /// Метод так же ограничивает одновременное количество исполняемых Task, по умолчанию - 4.
        /// </summary>
        /// <typeparam name="T"></typeparam> Тип возвращаемого объекта.
        /// <param name="maxTasks"> Определяет количество одновременно исполняемых потоков для получения объектов</param>
        /// <param name="throwException">Опциональный параметр, определяет будет ли выброшена первая ошибка при исполнении Task, остановлен метод и вызвана остановка оставшихся Task
        /// или метод вернет все полученные объекты вместе со всеми возникшими ошибками AggregateException. По умолчанию ошибка выброшена не будет.</param>
        /// <param name="tokenSource"> CancellationTokenSource на основе, которого были созданы CancellationToken'ы в переданных Task<T></param>
        /// <returns></returns>
        /// <exception cref="AggregateException"></exception>
        public static async IAsyncEnumerable<T> RunInParallelAlt<T>(this IEnumerable<Func<CancellationToken,Task<T>>> functs, int maxTasks = 4, bool throwException = false)
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            var semaphore = new SemaphoreSlim(maxTasks);
            var tasks = new List<Task<T>>();
            foreach (var func in functs)
            {
                var task = Task.Run(async () => {
                    await semaphore.WaitAsync(); //не стал добавлять токен, иначе семафор начинает себя странно вести
                    try
                    {
                        return await func.Invoke(token);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                }, token);
                tasks.Add(task);
            }
            var exList = new List<Exception>(tasks.Count);
            var taskIds = new HashSet<int>(tasks.Count);
            while (tasks.Any(x => x.Status == TaskStatus.Running || x.Status == TaskStatus.WaitingForActivation))
            {
                foreach (var task in tasks)
                {
                    if (taskIds.Contains(task.Id)) continue;
                    if (task.IsFaulted)
                    {
                        if (throwException)
                        {
                            tokenSource.Cancel();
                            throw new AggregateException(task.Exception);
                        }
                        exList.Add(task.Exception);
                        taskIds.Add(task.Id);
                    }
                    if (task.IsCompletedSuccessfully)
                    {
                        yield return await task;
                        taskIds.Add(task.Id);
                    }
                }
            }
            //финальный проход, на случай пропуска, который может возникнуть при прекращении while и неполном проходе foreach
            foreach (var task in tasks)
            {
                if (taskIds.Contains(task.Id)) continue;
                if (task.IsFaulted)
                {
                    exList.Add(task.Exception);
                }
                if (task.IsCompletedSuccessfully)
                {
                    yield return task.Result;
                }
            }
            if (exList.Count > 0) throw new AggregateException(exList);// возвращения списка ошибок, если они возникли
        }
    }
}
