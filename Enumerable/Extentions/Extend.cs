using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enumerable.Extentions
{
    public static class Extend
    {
        public static IEnumerable<T> CustomFilterGreatherThan<T>(this IEnumerable<T> collection, T GreatherThanValue) where T : IComparable<T>, IEquatable<T>
        {
            foreach (var value in collection)
            {
                if (value.CompareTo(GreatherThanValue) > 0)
                {
                    yield return value;
                }
            }
        }
        public static IEnumerable<T> CustomTakeWhile<T>(this IEnumerable<T> collection, Func<T, bool> Condition) where T : IComparable<T>, IEquatable<T>
        {
            foreach (var value in collection)
            {
                if (Condition.Invoke(value))
                {
                    yield return value;
                }
            }
        }
        public static IEnumerable<T> GenerateSequance<T>(this IEnumerable<T> Collection, T start, T step, T End) where T : IComparable<T>
        {
            T Current = start;
            while (Current.CompareTo(End) < 0)
            {
                yield return Current;
                Current = Add(Current, step);
            }
        }
        public static void PrintResult<T>(this IEnumerable<T> collection)
        {
            foreach (var item in collection)
            {
                Console.WriteLine(item);
            }
        }
        private static T Add<T>(T Current, T step)
        {
            dynamic currentDynamic = Current;
            dynamic stepDynamic = step;
            return currentDynamic + stepDynamic;
        }
    }
}
