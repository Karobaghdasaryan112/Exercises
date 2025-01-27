using CustomDicitonary.Service;

namespace CustomDictionary
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CustomDictionary<int, int> keyValuePairs = new CustomDictionary<int, int>();
            keyValuePairs.Add(1, 1);
            keyValuePairs.Add(2, 2);
            keyValuePairs.Add(3, 3);
            keyValuePairs.Add(4, 4);
            keyValuePairs.Add(5, 5);
            keyValuePairs.Add(6, 6);

            Console.WriteLine("_______________________");
            Console.WriteLine(keyValuePairs[1] = 1);
            Console.WriteLine("_______________________");
            keyValuePairs.Remove(6);
            foreach (var item in keyValuePairs)
            {
                Console.WriteLine(item.Pair.Key + "," + item.Pair.Value);

            }





            Console.WriteLine(keyValuePairs[5] = 1);
            Console.ReadLine();
        }
    }
}