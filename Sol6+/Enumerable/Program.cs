namespace Enumerable
{
    public class Program
    {
        public static void Main()
        {
            var tst = new MyEnumerable();
            foreach (var item in tst) 
                Console.WriteLine(item);
            Console.WriteLine("====================");
            var test2 = new MyIterator();
            foreach (var item in test2) 
                Console.WriteLine(item);
        }
    }
}