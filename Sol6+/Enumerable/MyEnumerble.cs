using System.Collections;

namespace Enumerable
{
    public class MyEnumerable : IEnumerable<int>
    {
        public IEnumerator<int> GetEnumerator() => new MyEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => new MyEnumerator();
    }
    public class MyEnumerator : IEnumerator<int>
    {
        private int index = -1;
        private bool doReverse = false;
        public int Current => index;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (doReverse)
            {
                if (index == 0) return false;
                index--;
                return true;
            }
            index++;
            if (index == 100) doReverse = true;
            return true;
        }

        public void Reset()
        {
        }
    }
}
