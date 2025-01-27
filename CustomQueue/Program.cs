using CustomQueue.Service;

namespace CustomQueue
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CustomQueue<int> customQueue = new CustomQueue<int>();
            customQueue.Enqueue(3);
            customQueue.Enqueue(4);
            customQueue.Enqueue(5);
            customQueue.Enqueue(6);
            foreach (var item in customQueue)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("__________________");
            customQueue.TryDequeue(out int value);
            customQueue.TryDequeue(out int value1);
            customQueue.TryDequeue(out int value2);
            customQueue.TryDequeue(out int value3);
            customQueue.TryDequeue(out int value4);
            Console.WriteLine(value);
            Console.WriteLine(value1);
            Console.WriteLine(value2);
            Console.WriteLine(value3);
            Console.WriteLine(value4);
            IEnumerable<int> ints = new int[] {1,2,3,4,5};
            CustomQueue<int> custom = new CustomQueue<int>(ints);
            foreach (var item in custom)
            {
                Console.WriteLine(item);
            }
            Console.ReadLine();
        }
    }
}
