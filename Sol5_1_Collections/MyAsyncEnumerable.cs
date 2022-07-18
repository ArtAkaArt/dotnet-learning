namespace Sol5_1_Collections
{
    public class MyAsyncEnumerable<T> : IAsyncEnumerable<T>
    {
        private readonly IEnumerable<Func<CancellationToken, Task<T>>>? listWithToken;
        private readonly IEnumerable<Func<Task<T>>>? list;
        private readonly int maxTasks;
        private readonly ErrorsHandleMode mode;
        private readonly int capacity;

        public MyAsyncEnumerable(IEnumerable<Func<CancellationToken, Task<T>>> list, int maxTasks = 4, ErrorsHandleMode mode = ErrorsHandleMode.ReturnAllErrors, int capacity = 10) 
        {
            this.capacity = capacity;
            this.maxTasks = maxTasks;
            this.mode = mode;
            listWithToken = list;
        }
        public MyAsyncEnumerable(IEnumerable<Func<Task<T>>> list, int maxTasks = 4, ErrorsHandleMode mode = ErrorsHandleMode.ReturnAllErrors, int capacity = 10)
        {
            this.capacity = capacity;
            this.maxTasks = maxTasks;
            this.mode = mode;
            this.list = list;
        }
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            if (listWithToken is not null)
                return new MyAsyncEnumerator<T>(listWithToken, maxTasks, mode, capacity);
            return new MyAsyncEnumerator<T>(list!, maxTasks, mode, capacity);
        }
    }
}
