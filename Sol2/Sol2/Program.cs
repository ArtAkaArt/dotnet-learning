namespace Solution
{
    class Program
    {
        public static void Main()
        {
            var part1 = Enumerable.Range(15_000, 100_000);
            var part2 = Enumerable.Range(0, 14_995);
            var sample = part1.Concat(part2).ToArray(); //expected: 100_000
            var sample1 = new[] { 15, 16, 18, 20, 1, 2, 5, 6, 7, 8, 11, 12 }; // expected: 4
            var sample2 = new[] { 5, 6, 7,     8, 11, 12,     15, 16, 18,     1, 2, 3 }; // expected: 9
            var sample3 = new[] { 1, 2, 5, 6, 7, 8, 11, 12, 15, 16, 18, 20 }; // expected: 0
            var sample4 = new[] { 11, 1, 2, 5 }; //малый четный массив, expected: 1
            var sample5 = new[] { 7, 11, 1, 2, 5 }; //малый нечетный массив, expected: 2
            var sample6 = new[] { 5, 6, 1, 2}; // малый со сдвигом в центре, expected: 2
            var sample7 = new[] { 5 }; // expected: 0

            Console.WriteLine($"В массиве sample сдвиг равен = {GetShiftPositionAlt(sample)}");
            Console.WriteLine($"В массиве sample1 сдвиг равен = {GetShiftPositionAlt(sample1)}");
            Console.WriteLine($"В массиве sample2 сдвиг равен = {GetShiftPositionAlt(sample2)}");
            Console.WriteLine($"В массиве sample3 сдвиг равен = {GetShiftPositionAlt(sample3)}");
            Console.WriteLine($"В массиве sample4 сдвиг равен = {GetShiftPositionAlt(sample4)}");
            Console.WriteLine($"В массиве sample5 сдвиг равен = {GetShiftPositionAlt(sample5)}");
            Console.WriteLine($"В массиве sample6 сдвиг равен = {GetShiftPositionAlt(sample6)}");
            Console.WriteLine($"В массиве sample7 сдвиг равен = {GetShiftPositionAlt(sample7)}");
        }
        static int GetShiftPositionAlt(int[] arr)
        {
            if (arr[0] < arr[arr.Length - 1] || arr.Length == 1) return 0;
            var start = 0;
            var end = arr.Length-1;
            while (end - start > 3)
            {
                if (arr[start] > arr[((end-start) / 2)+start]) end = start + (end-start)/2; 
                else if (arr[((end - start) / 2) + start + 1] > arr[end]) start = ((end - start) / 2) + start + 1;
                else if (arr[((end - start) / 2) + start ] > arr[((end - start) / 2) + start + 1]) return ((end - start) / 2) + start + 1; //багуля
            }
            var buffer = int.MaxValue;
            int result = 0;
            for (int i = start; i<=end; i++)
            {
                if (arr[i] < buffer)
                {
                    buffer = arr[i];
                    result = i;
                }
            }
            return result;
        }
        static int FindLowest(int[] arr)
        {
            if (arr[0] < arr[arr.Length - 1] || arr.Length == 1) return arr[0];
            var result = int.MaxValue;
            if (arr.Length == 3 || arr.Length == 2) 
            {
                foreach (int t in arr)
                {
                    if (t < result)
                    { 
                        result = t;
                    }
                }
            }
            var end = arr.Length / 2;
            var start = arr.Length / 2 + 1;

            if (arr[0] > arr[end-1])
            {
                var firstHalf = new int[end];
                Array.Copy(arr, firstHalf, end);
                result = FindLowest(firstHalf);
            }
            else if (arr[start-1] > arr[arr.Length - 1])
            {
                var secondHalf = new int[arr.Length - end];
                Array.Copy(arr, end, secondHalf, 0, arr.Length - end);
                result = FindLowest(secondHalf);
            }
            else if (arr[end-1] > arr[start-1]) result = arr[end];
            return result;
        }

        static int GetShiftPosition(int[] arr)
        {
            return Array.IndexOf(arr, FindLowest(arr));
        }
        
    }
}