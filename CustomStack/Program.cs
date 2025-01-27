using CustomStack.Service;
using System.Runtime.Intrinsics.X86;

namespace CustomStack
{
    public class Program
    {
        public static void Main(string[] args)
        {
           CustomStack<int> Stack = new CustomStack<int>();
            Stack.Push(1);
            Stack.Push(2);
            Stack.Push(3);
            Stack.Push(4);
            Stack.Push(5);
            foreach (var item in Stack)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine(Stack.Pop());
            Console.WriteLine(Stack.Pop());
            Console.WriteLine(Stack.Pop());
            Console.WriteLine(Stack.Pop());
            Console.WriteLine(Stack.Pop());
            Console.WriteLine(Stack.TryPop(out int value));
            
            IEnumerable<int> ints = new int[] { 1, 2, 3, 4, 5 };
            CustomStack<int> Stack1 = new CustomStack<int>(ints);
            foreach (var item in Stack1)
            {
                Console.WriteLine(item);
            }
            Console.ReadLine();
        }
    }
}
