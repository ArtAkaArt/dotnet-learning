using System.Diagnostics;

namespace Sol5_1_Collections
{
    internal class MyAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private bool isStarted = true;
        private readonly ErrorsHandleMode mode;
        private readonly IEnumerable<Func<CancellationToken, Task<T>>> funcList;
        private readonly CancellationTokenSource cts = new();
        private readonly SemaphoreSlim semaphore;
        private List<Task<T>> taskCollection;
        private readonly List<Exception> exceptions = new();
        public T? Current { get; private set; }

        T IAsyncEnumerator<T>.Current => Current;

        public MyAsyncEnumerator(IEnumerable<Func<CancellationToken, Task<T>>> list, int maxTasks, ErrorsHandleMode mode, int capacity)
        {
            funcList = list;
            taskCollection = new(capacity);
            semaphore = new SemaphoreSlim(maxTasks);
            this.mode = mode;
        }
        public ValueTask DisposeAsync()
        {
            cts.Cancel();
            return ValueTask.CompletedTask;
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            if (isStarted) 
                StrartAllTasks();

            isStarted = false;
            while (taskCollection.Count != 0)
            {
                var endedTask = await Task.WhenAny(taskCollection);
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
                            exceptions.Add(endedTask.Exception);
                            break;
                        case ErrorsHandleMode.EndAtFirstError:
                            throw endedTask.Exception;
                    }
                }
                else if (endedTask.Status == TaskStatus.Canceled)
                {
                    //не делаю проверку mode, т к если уж юзер явно вызвал отмену тасок, то наверно надо просто все отменить, иначе зачем он ее вызывал?
                    cts.Cancel(); // да  и надо ли это, он же уже вызван
                    throw new AggregateException(new TaskCanceledException("Вызван cancelation token"));
                }
                else Debug.Assert(false);
            }
            if (exceptions.Count > 0) throw new AggregateException(exceptions);
            
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
            }
        }
    }
}
