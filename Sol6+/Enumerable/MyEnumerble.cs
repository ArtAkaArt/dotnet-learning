using System.Collections;

namespace Enumerable
{
    public class MyEnumerable : IEnumerable<int>
    {
<<<<<<< HEAD
        public IEnumerator<int> GetEnumerator() => new MyEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
=======
        public IEnumerator<int> GetEnumerator()
        {
            return new MyEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new MyEnumerator();
        }
>>>>>>> 5c50d95 (v1 Enumerable & Iterator)
    }
    public class MyEnumerator : IEnumerator<int>
    {
        private int index = -1;
        private bool doReverse = false;
<<<<<<< HEAD
        public int Current => index;
=======
        public int Current { get { return index; } }

>>>>>>> 5c50d95 (v1 Enumerable & Iterator)
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
