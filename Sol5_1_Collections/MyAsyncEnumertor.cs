namespace Sol5_1_Collections
{
    internal class MyAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerable<Func<CancellationToken, Task<T>>> funcList;
        private int position = 0;
        private int size = 0;
        private readonly CancellationTokenSource cts;
        private List<Task<T>> taskCollection;
        private Task<T> endedTask;
        private readonly SemaphoreSlim semaphore;
        public T? Current { get; private set; }
        private readonly List<Exception> exList;
        private readonly ErrorsHandleMode mode;

        T IAsyncEnumerator<T>.Current => Current;

        public MyAsyncEnumerator(IEnumerable<Func<CancellationToken, Task<T>>> list, int maxTasks, ErrorsHandleMode mode, int initialSize = 10)
        {
            funcList = list;
            cts = new CancellationTokenSource();
            taskCollection = new(initialSize);
            semaphore = new SemaphoreSlim(maxTasks);
            exList = new();
            this.mode = mode;
        }
        public ValueTask DisposeAsync()
        {
            cts.Cancel();
            return ValueTask.CompletedTask;
        }

        public async ValueTask<bool> MoveNextAsync()
        {
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
                    switch (mode)
                    {
                        case ErrorsHandleMode.IgnoreErrors:
                            break;
                        case ErrorsHandleMode.ReturnAllErrors:
                            exList.Add(endedTask.Exception);
                            break;
                        case ErrorsHandleMode.EndAtFirstError:
                            throw endedTask.Exception;
                    }
                }
                else if (endedTask.Status == TaskStatus.Canceled)
                {
                    //не делаю проверку mode, т к если уж юзер явно вызвал отмену тасок, то наверно надо просто все отменить, иначе зачем он ее вызывал?
                    cts.Cancel(); // да  и надо ли это, он же уже вызван
                    throw new AggregateException(new Exception("Вызван cancelation token"));
                }
            }
            if (taskCollection.Count == 0 && exList.Count > 0) throw new AggregateException(exList);
            if (position >= size || size == 0) return false;
            
            return false;
        }

        private void StrartAllTasks()
        {
            var token = cts.Token;
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
                size++;
            }
        }
    }
}
