using System.Collections.Concurrent;


namespace Sol5_1_Collections
{
    internal class MyAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerable<Func<CancellationToken, Task<T>>> funcList;
        private int position;
        private readonly int size;
        private readonly CancellationTokenSource cts;
        private readonly CancellationToken token;
        private List<Task<T>> taskCollection;
        private Task<T> endedTask;
        private readonly SemaphoreSlim semaphore;
        public T? Current { get; private set; }
        private readonly List<Exception> exList;
        private readonly bool throwEx;

        T IAsyncEnumerator<T>.Current => Current;

        public MyAsyncEnumerator(IEnumerable<Func<CancellationToken, Task<T>>> list, int maxTasks, bool throwEx)
        {
            funcList = list;
            position = 0;
            size = list.Count(); // чтобы оставить IEnumerable и не перебирать каждый раз, записал в отдельное поле
            cts = new CancellationTokenSource();
            token = cts.Token;
            taskCollection = new(size);
            semaphore = new SemaphoreSlim(maxTasks);
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
            if (taskCollection.Count == 0 && exList.Count > 0) throw new AggregateException(exList);
            if (position >= size || size == 0) return false;
            if (position == 0) StrartAllTasks();

            while (taskCollection.Count != 0)
            {
                position++;
                endedTask = await Task.WhenAny(taskCollection);
                taskCollection.Remove(endedTask);
                if (endedTask.Status == TaskStatus.RanToCompletion)
                {
                    Current = endedTask.Result;
                    return true;
                }
                else if (endedTask.Status == TaskStatus.Faulted)
                {
                    exList.Add(endedTask.Exception);
                    if (throwEx) throw endedTask.Exception;
                }
            }
            if (taskCollection.Count == 0 && exList.Count > 0) throw new AggregateException(exList);
            return false;
        }

    private void StrartAllTasks()
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
}
}
