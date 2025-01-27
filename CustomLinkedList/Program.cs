using CustomLinkedList.Services;

namespace CustomHashSet
{
    public class Program 
    {
        public static void Main(string[] args)
        {
            CustomLinkedList<int> ints = new CustomLinkedList<int>();
            ints.Add(1);
            ints.Add(2);
            ints.Add(3);
            ints.Add(4);
            ints.Add(5);
            ints.Add(6);
            ints.Add(7);


            ints.AddLast(8);
            ints.AddAfter(ints._first._next._next, 10);
            ints.AddBefore(ints._first._next._next, 10);
            foreach (var item in ints)
            {
                Console.WriteLine(item);
            }

            Console.ReadLine();
        }
    }
}