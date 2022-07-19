using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enumerable
{
    public class MyIterator
    {
        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i <= 100; i++)
            {
                yield return i;
            }
            for (int i = 99; i >= 0; i--)
            {
                yield return i;
            }
        }
    }
}
