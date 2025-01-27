using CustomHashSet.Service;

namespace CustomHashSet
{
    public class Program
    {
        public static void Main(string[] args)
        {
           CustomHashSet<int> ints = new CustomHashSet<int>();
            ints.Add(1);    
            ints.Add(2);
            ints.Add(3);
            ints.Add(1);

            foreach (var item in ints)
            {
                Console.WriteLine(item);
            }
            Console.ReadLine();
        }
    }
}
