using Enumerable.Extentions;
using Enumerable.Services;

namespace Enumerable
{
    public class Program 
    {
        public static void Main(string[] args)
        {
            CustomList<int> ints = new CustomList<int>();
            ints.AddToList(1);
            ints.AddToList(2);
            ints.AddToList(3);
            ints.AddToList(4);
            ints.AddToList(5);
            ints.PrintResult();

            Console.WriteLine("_______________________________________\n");

            List<int> integers = new List<int>();

            integers.Add(1);
            integers.Add(2);
            integers.Add(3);
            integers.Add(4);
            integers.Add(5);
            integers.Add(6);
            integers.Add(7);

            var FilteredEnumerable =
                integers.
                CustomFilterGreatherThan(1).
                CustomTakeWhile(x => x < 6);
            FilteredEnumerable.PrintResult();

            Console.WriteLine("_______________________________________\n");

            var integer = integers.GenerateSequance(2, 2, 5);

            integer.PrintResult();

            Console.ReadLine();
        }
    }
}