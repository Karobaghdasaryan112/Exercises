
namespace LINQtoObject.Service
{
    public static class Extentions
    {

        //_____________________________________________________________________________________________________________
        public static IEnumerable<string> GreatherThanMinLengthOutput(this IEnumerable<string> strings, int MinLength)
        {
            foreach (var item in strings)
            {
                if (item.Length > MinLength)
                {
                    yield return item;
                }
            }
        }


        //_____________________________________________________________________________________________________________
        public static IEnumerable<T> FilterWithIndex<T>(this IEnumerable<T> values, int index)
        {
            int i = 0;
            foreach (var item in values)
            {
                if (i % index == 0)
                    yield return item;

                i++;
            }
        }
        //_____________________________________________________________________________________________________________


        public static IEnumerable<int> PrimeNumbersCollection(this IEnumerable<int> values)
        {
            foreach (var item in values)
            {
                if (IsPrime(item))
                    yield return item;
            }
        }
        private static bool IsPrime(int Number)
        {
            if (Number < 2)
                return false;

            for (int i = 2; i <= Math.Sqrt(Number); i++)
            {
                if (Number % i == 0)
                {
                    return false;
                }
            }
            return true;
        }
        //_____________________________________________________________________________________________________________

        public static int MaxElementWithCondition(this IEnumerable<int> values, Func<int, bool> Condition)
        {
            int Max = int.MinValue;
            foreach (var item in values)
            {
                if (Condition(item))
                {
                    if (Max < item)
                        Max = item;
                }
            }
            return Max;
        }

    }
}
