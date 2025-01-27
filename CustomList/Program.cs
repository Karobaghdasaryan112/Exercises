using CustomList.Service;

namespace CustomList
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CustomList<int> customList = new CustomList<int>();
            customList.Add(1);
            customList.Add(2);
            customList.Add(3);
            customList.Add(4);
            foreach (var item in customList)
            {
                Console.WriteLine(item);
            }
            customList.Remove(1);
            Console.WriteLine("_____________________________________");
            foreach (var item in customList)
            {
                Console.WriteLine(item);
                
            }
            Console.WriteLine("_____________________________________");
            customList.Insert(1, 2);
            foreach (var item in customList)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("_____________________________________");
            CustomList<int> CustomListWithParams = new CustomList<int>() { 1,2,3,4 };
            foreach (var item in CustomListWithParams)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("_____________________________________");
            IEnumerable<int> ints = new int[] { 1, 2, 3, 4 };
            CustomList<int> CustomListWithParams1 = new CustomList<int>(ints);
            foreach (var item in CustomListWithParams1)
            {
                Console.WriteLine(item);
            }
            Console.ReadLine();
        }
    }
}
