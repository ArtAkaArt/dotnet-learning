using System.Collections.Concurrent;


namespace Sol5_1_Collections
{
    internal class MyAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly List<Func<CancellationToken, Task<T>>> funcList;
        private int position;
        private readonly CancellationTokenSource cts;
        private readonly CancellationToken token;
        private List<Task<T>> taskCollection;
        private Task<T> endedTask;
        private readonly SemaphoreSlim semaphore;
        public T? Current { get; private set; }
        private readonly List<Exception> exList;
        private static object syncObject;
        private bool throwEx;

        T IAsyncEnumerator<T>.Current => Current;

        public MyAsyncEnumerator(IEnumerable<Func<CancellationToken, Task<T>>> list, int maxTasks, bool throwEx)
        {
            funcList = (List<Func<CancellationToken, Task<T>>>?)list;
            position = -1;
            cts = new CancellationTokenSource();
            token = cts.Token;
            taskCollection = new(funcList.Count);
            semaphore = new SemaphoreSlim(maxTasks);
            syncObject = new();
            exList = new();
            this.throwEx = throwEx;
        }

        public ValueTask DisposeAsync()
        {
            cts.Cancel();
            return ValueTask.CompletedTask;
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            if (position >= funcList.Count -1 || funcList.Count == 0) return false;

            if (position == -1)
            {
                foreach (var func in funcList)
                {
                    var task = Task.Run(async () =>
                    {
                        await semaphore.WaitAsync();
                        try
                        {
                            return await func.Invoke(token);
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }, token);
                    taskCollection.Add(task);
                }
            }

            while (true)
            {
                try
                {
                    endedTask = await Task.WhenAny(taskCollection);
                    taskCollection.Remove(endedTask);
                    Current = await endedTask;
                    return true;
                    
                }
                catch (Exception ex)
                {
                    exList.Add(ex);
                    taskCollection.Remove(endedTask);
                    if (throwEx) throw new AggregateException(ex);
                    if (taskCollection.Count == 0 && exList.Count > 0) throw new AggregateException(exList);
                    continue;
                }
                finally
                {
                    position++;
                    Console.WriteLine(position+" позиция");
                }
            }
        }
    }
}
