namespace Sol5_1_Collections
{
    public class MyAsyncEnumerable<T> : IAsyncEnumerable<T>
    {
        private readonly IEnumerable<Func<CancellationToken, Task<T>>> posts;
        private int maxTasks;
        private ErrorsHandleMode mode;
        private int capacity;

        public MyAsyncEnumerable(IEnumerable<Func<CancellationToken, Task<T>>> list, int maxTasks = 4, ErrorsHandleMode mode = ErrorsHandleMode.ReturnAllErrors, int capacity = 10) 
        {
            this.capacity = capacity;
            this.maxTasks = maxTasks;
            this.mode = mode;
            posts = list;
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
             return new MyAsyncEnumerator<T>(posts, maxTasks, mode, capacity);
        }
    }
}
