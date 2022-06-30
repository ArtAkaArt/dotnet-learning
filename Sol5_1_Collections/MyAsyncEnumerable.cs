using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sol5_1_Collections
{
    internal class MyAsyncEnumerable<T> : IAsyncEnumerable<T>
    {
        private readonly IEnumerable<Func<CancellationToken, Task<T>>> posts;
        private int maxTasks;

        public MyAsyncEnumerable(IEnumerable<Func<CancellationToken, Task<T>>> list, int maxTasks = 4) 
        {
            posts = list;
            this.maxTasks = maxTasks;
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
             return new MyAsyncEnumerator<T>(posts, maxTasks);
        }
    }
}
