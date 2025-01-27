using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace CustomHashSet.Service
{
    public class CustomHashSet<T> : ICollection<T>, ISet<T>, IReadOnlyCollection<T>, IReadOnlySet<T>, IEqualityComparer<T>
    {
        private List<T>[]? _buckets { get; set; }
        private int _capacity { get; set; }
        private int _arrayResizeCount { get; set; }

        private IEqualityComparer<T>? _comparer;


        public CustomHashSet()
        {
            Initialize();
        }

        public CustomHashSet(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            _capacity = capacity;
            _buckets = new List<T>[_capacity];
        }

        public CustomHashSet(IEqualityComparer<T> comparer)
        {
            _comparer = comparer;
        }

        public CustomHashSet(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            _capacity = collection.Count();
            _buckets = new List<T>[_capacity];

            foreach (var item in collection)
            {
                Add(item);
            }
        }

        public CustomHashSet(IEnumerable collection, IEqualityComparer<T> comparer)
        {
            _comparer = comparer;

        }
        private bool IsArrayResize()
        {
            int ColisionCount = 0;
            int ValuesCount = 0;
            foreach (var item in _buckets)
            {
                if (item != null)
                {
                    ValuesCount += item.Count;
                    if (item != null && item.Count > 1)
                    {
                        ColisionCount += item.Count() - 1;
                    }
                }
            }

            if (ColisionCount > ValuesCount / 2)
                return false;

            if (ColisionCount * 0.75 >= _arrayResizeCount && ColisionCount != 0)
            {
                return true;
            }

            return false;

        }
        private void ArrayResize()
        {
            _capacity *= 2;
            var NewBuckets = new List<T>[_capacity];
            Array.Copy(_buckets, NewBuckets, _buckets.Length);
            _buckets = NewBuckets;
        }

        private void Initialize()
        {
            _capacity = 4;
            _buckets = new List<T>[_capacity];
            for (int i = 0; i < _buckets.Length; i++)
            {
                _buckets[i] = new List<T>();
            }
        }

        public int Count => _capacity;

        public bool IsReadOnly => false;


        public void Add(T value)
        {
            if (IsArrayResize())
                ArrayResize();


            var Hash = _comparer?.GetHashCode() ?? value.GetHashCode();
            int index = Math.Abs(Hash) % Count;
            if (_comparer == null)
            {
                foreach (var item in _buckets[index])
                {
                    if (item.Equals(value))
                        return;
                }
                _buckets[index].Add(value);

            }
            else
            {
                if (_buckets[index] == null)
                    _buckets[index] = new List<T>() { value };


                foreach (var item in _buckets[index])
                {
                    if (_comparer.Equals(item, value))
                        return;
                }
                _buckets[index].Add(value);

            }
        }

        public void Clear() => Initialize();


        public bool Contains(T item)
        {
            if (item == null)
                return false;

            var HashCode = _comparer?.GetHashCode(item) ?? item.GetHashCode();
            var IsEqual = false;
            int index = Math.Abs(HashCode) % Count;
            if (_buckets[index] == null)
                return false;
            foreach (var value in _buckets[index])
            {
                IsEqual = _comparer?.Equals(value, item) ?? Equals(value, item);
                if (IsEqual)
                    return true;
            }
            return IsEqual;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));


            int totalCount = 0;
            foreach (var bucket in _buckets)
            {
                if (bucket != null)
                    totalCount += bucket.Count;
            }
            T[] list = new T[totalCount];

            if (arrayIndex + totalCount > array.Length)
                throw new ArgumentException("Insufficient space in the array to copy the elements.");

            if (arrayIndex + list.Length - 1 < array.Length)
            {
                Array.Copy(list, arrayIndex, array, 0, list.Length);
            }

        }

        public void ExceptWith(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            foreach (var item in other)
            {
                if (Contains(item))
                {
                    Remove(item);
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new CustomEnumerator<T>(_buckets);
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));


            foreach (var item in other)
            {
                if (!Contains(item))
                {
                    Remove(item);
                }
            }

        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            int ContainsCount = 0;
            int ValuesCount = 0;
            foreach (var bucket in _buckets)
            {
                ValuesCount += bucket.Count();
            }
            if (ValuesCount > other.Count())
                return false;

            foreach (var item in other)
            {
                if (Contains(item))
                    ContainsCount++;
            }

            if (ValuesCount == ContainsCount)
                return true;

            return false;

        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            if(other == null)
                throw new ArgumentNullException(nameof(other));

            int ValuesCount = 0;
            int OtherCount = other.Count();
            int ContainsCount = 0;
            foreach (var bucket in _buckets)
            {
                ValuesCount += bucket.Count();
            }

            if (OtherCount >= ValuesCount)
                return false;

            foreach (var item in other)
            {
                if (Contains(item))
                    ContainsCount++;
            }
            if (ContainsCount == OtherCount)
                return true;
            return false;

        }
        public bool IsSupersetOf(IEnumerable<T> other)
        {
            if(other == null)
                throw new ArgumentNullException(nameof(other));

            int ValuesCount = 0;
            int OtherCount = other.Count();
            int ContainsCount = 0;

            foreach (var bucket in _buckets)
            {
                ValuesCount += bucket.Count();
            }
            if (OtherCount <= ValuesCount)
                return false;

            foreach (var item in other)
            {
                if (Contains(item))
                    ContainsCount++;
            }

            if (ContainsCount == OtherCount)
                return true;

            return false;
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            foreach (var item in other)
            {
                if (Contains(item))
                    return true;
            }
            return false;
        }

        public bool Remove(T item)
        {
            if (item == null)
                return false;

            if (Contains(item))
            {
                foreach (var bucket in _buckets)
                {
                    if (bucket.Contains(item))
                    {
                        bucket.Remove(item);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            int ContainsCount = 0;
            int otherCount = other.Count();
            int ValuesCount = 0;

            foreach (var item in other)
            {
                if (Contains(item))
                    ContainsCount++;
            }

            if (otherCount == ContainsCount)
            {
                foreach (var bucket in _buckets)
                {
                    ValuesCount += bucket.Count();
                }

                if (ValuesCount == otherCount)
                    return true;

            }
            return false;
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            foreach (var item in other)
            {
                if (Contains(item))
                {
                    Remove(item);
                }
                Add(item);
            }
        }

        public void UnionWith(IEnumerable<T> other)
        {
            foreach (var item in other)
            {
                Add(item);
            }
        }

        bool ISet<T>.Add(T value)
        {
            if (IsArrayResize())
                ArrayResize();


            var Hash = _comparer?.GetHashCode() ?? value.GetHashCode();
            int index = Math.Abs(Hash) % Count;
            if (_comparer == null)
            {
                foreach (var item in _buckets[index])
                {
                    if (item.Equals(value))
                        return false;
                }
                _buckets[index].Add(value);

            }
            else
            {
                if (_buckets[index] == null)
                    _buckets[index] = new List<T>() { value };


                foreach (var item in _buckets[index])
                {
                    if (_comparer.Equals(item, value))
                        return false;
                }
                _buckets[index].Add(value);

            }
            return true;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool Equals(T? x, T? y)
        {
            if (x == null || y == null)
                return false;

            if (x.Equals(y))
                return true;

            return false;
        }

        public int GetHashCode([DisallowNull] T obj)
        {
            return obj.GetHashCode();
        }
    }
    public class CustomEnumerator<T> : IEnumerator<T>
    {
        private T _current;
        private List<T>[] _buckets;
        private int _bucketIndex = 0;
        public CustomEnumerator(List<T>[] buckets)
        {
            _buckets = buckets;
        }
        public T Current => _current;

        object IEnumerator.Current => _current;

        public void Dispose()
        {
            _buckets = default(List<T>[]);
            _current = default;
        }

        public bool MoveNext()
        {
            while (_bucketIndex < _buckets.Length)
            {
                foreach (var bucket in _buckets[_bucketIndex])
                {
                    _current = bucket;
                    _bucketIndex++;
                    return true;
                }
                _bucketIndex++;
            }
            return false;
        }

        public void Reset()
        {
            
        }
    }
}
