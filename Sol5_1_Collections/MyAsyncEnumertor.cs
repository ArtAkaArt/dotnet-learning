using System.Collections.Concurrent;


namespace Sol5_1_Collections
{
    internal class MyAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly List<Func<CancellationToken, Task<T>>> funcList;
        private int position;
        private readonly CancellationTokenSource cts;
        private readonly CancellationToken token;
        //private BlockingCollection<Task<Post>> bag;
        private List<Task<T>> taskCollection;
        private Task<T> endedTask;
        private readonly SemaphoreSlim semaphore;
        public T? Current { get ; private set; }
        private readonly List<Exception> exList;
        private static Object syncObject;

        T IAsyncEnumerator<T>.Current => Current;

        public MyAsyncEnumerator(IEnumerable<Func<CancellationToken, Task<T>>> list, int maxTasks)
        {
            funcList = (List<Func<CancellationToken, Task<T>>>?)list;
            position = -1;
            cts = new CancellationTokenSource();
            token = cts.Token;
            taskCollection = new(funcList.Count);
            semaphore = new SemaphoreSlim(maxTasks);
            syncObject = new Object();
        }

        public ValueTask DisposeAsync()
        {
            cts.Cancel();
            return ValueTask.CompletedTask;
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            if (position >= funcList.Count-1) return false;
            
            if (position == -1)
            {
                foreach (var func in funcList)
                {
                    var task = Task.Run(async () => {
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
            position++;
            endedTask = await Task.WhenAny(taskCollection);
            lock (syncObject)
            {
                taskCollection.Remove(endedTask);
            }
            Current = await endedTask;
            return true;
        }
    }
}