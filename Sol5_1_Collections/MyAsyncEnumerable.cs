namespace Sol5_1_Collections
{
    public class MyAsyncEnumerable<T> : IAsyncEnumerable<T>
    {
        private readonly IEnumerable<Func<CancellationToken, Task<T>>> posts;
        private int maxTasks;
        private ErrorsHandleMode mode;

        public MyAsyncEnumerable(IEnumerable<Func<CancellationToken, Task<T>>> list, int maxTasks = 4, ErrorsHandleMode mode = ErrorsHandleMode.ReturnAllErrors) 
        {
            posts = list;
            this.maxTasks = maxTasks;
            this.mode = mode;
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
             return new MyAsyncEnumerator<T>(posts, maxTasks, mode);
        }
    }
}
