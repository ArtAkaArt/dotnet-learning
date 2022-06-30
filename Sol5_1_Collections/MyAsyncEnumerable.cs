﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sol5_1_Collections
{
    internal class MyAsyncEnumerable : IAsyncEnumerable<Post>
    {
        private readonly List<Func<CancellationToken, Task<Post>>> _posts;

        public MyAsyncEnumerable(IEnumerable<Func<CancellationToken, Task<Post>>> list) {
            _posts = (List<Func<CancellationToken, Task<Post>>>?)list;
                }

        public IAsyncEnumerator<Post> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
             return new MyAsyncEnumerator(_posts);
        }
    }
}