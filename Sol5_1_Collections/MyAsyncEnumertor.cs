namespace Sol5_1_Collections
{
    internal class MyAsyncEnumerator : IAsyncEnumerator<Post>
    {
        private readonly List<Func<CancellationToken, Task<Post>>> postList;
        private int position;
        private readonly CancellationTokenSource cts;
        private readonly CancellationToken cancellationToken;
        public Post Current { get ; private set; }

        Post IAsyncEnumerator<Post>.Current => Current;

        public MyAsyncEnumerator(IEnumerable<Func<CancellationToken, Task<Post>>> list)
        {
            postList = (List<Func<CancellationToken, Task<Post>>>?)list;
            position = -1;
            cts = new CancellationTokenSource();
            cancellationToken = cts.Token;
        }

        public ValueTask DisposeAsync()
        {
            cts.Cancel();
            return ValueTask.CompletedTask;
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            if (position >= postList.Count-1) return false;
            
            position++;
            //var localPosition = position;
            Current = await Task.Run(async () => {
                    return await postList[position].Invoke(cancellationToken);
            }, cancellationToken);
            //position = localPosition;
            Console.WriteLine("iter");
            return true;
        }
    }
}