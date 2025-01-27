using System.Collections;
using System.Diagnostics.CodeAnalysis;


namespace CustomDicitonary.Service
{
    public struct Bucket<TKey, TValue>
    {
        public bool IsOccupied { get; set; }
        public KeyValuePair<TKey, TValue> Pair { get; set; }
    }

    public class CustomDictionary<TKey, TValue> :
        ICollection<Bucket<TKey, TValue>>,
        IEnumerable<Bucket<TKey, TValue>>,
        IEnumerable,
        IDictionary<TKey, TValue>,
        IReadOnlyCollection<Bucket<TKey, TValue>>,
        IReadOnlyDictionary<TKey, TValue>, ICollection
        where TKey : notnull
    {
        private Bucket<TKey, TValue>[]? _buckets;
        private IEqualityComparer<TKey>? _comparer { get; set; }
        private IDictionary<TKey, TValue>? _dictionary { get; set; }

        private IEnumerable<Bucket<TKey, TValue>>? _enumerableKeyValues;

        private int _capacity { get; set; }
        private int _count { get; set; }

        public int Count => _count;

        public bool IsReadOnly => false;

        public ICollection<TKey> Keys => throw new NotImplementedException();

        public ICollection<TValue> Values => throw new NotImplementedException();

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => throw new NotImplementedException();

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => throw new NotImplementedException();

        public bool IsSynchronized => true;

        public object SyncRoot => new object();

        public TValue this[TKey key]
        {
            get
            {
                var index = GetBucketIndex(key, _buckets.Length);

                if (_comparer?.Equals(key, _buckets[index].Pair.Key)
                    ?? Equals(key, _buckets[index].Pair.Key)
                    && _buckets[index].IsOccupied)
                {
                    return _buckets[index].Pair.Value;
                }
                throw new KeyNotFoundException($"{key} this key is not found");
            }
            set
            {
                var index = GetBucketIndex(key, _buckets.Length);
                do
                {
                    if (!_buckets[index].IsOccupied)
                    {
                        SetPair(key, value, index);
                        _count++;
                        return;
                    }
                    else
                    {

                        if (_comparer?.Equals(key, _buckets[index].Pair.Key) ?? Equals(key, _buckets[index].Pair.Key))
                        {
                            SetPair(key, value, index);
                            _count--;
                            return;
                        }
                        index = (index + 1) % _buckets.Length;
                        if (index == _buckets.Length - 1 && _count == _buckets.Length)
                        {
                            ArrayResize();
                            index = GetBucketIndex(key, _buckets.Length);
                        }
                    }
                } while (index != _buckets.Length - 1);
            }
        }

        public CustomDictionary()
        {
            
            Initialize();
        }
        
        public CustomDictionary(int Capacity)
        {
            _capacity = Capacity;
        }
        public CustomDictionary(IEqualityComparer<TKey>? equalityComparer)
        {
            if (equalityComparer == null)
                throw new ArgumentNullException(nameof(equalityComparer));

            _comparer = equalityComparer;
        }
        public CustomDictionary(IEnumerable<Bucket<TKey, TValue>>? EnumerablekeyValues)
        {
            if (EnumerablekeyValues == null)
                throw new ArgumentNullException(nameof(EnumerablekeyValues));

            foreach (var item in EnumerablekeyValues)
            {
                Add(item);
            }
        }

        public CustomDictionary(int Capacity, IEqualityComparer<TKey>? equalityComparer)
        {
            if (equalityComparer == null)
                throw new ArgumentNullException(nameof(equalityComparer));

            if (Capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(Capacity));


        }
        public CustomDictionary(IEnumerable<Bucket<TKey, TValue>>? EnumerablekeyValues, IEqualityComparer<TKey>? equalityComparer)
        {

        }
        private void Initialize()
        {
            _capacity = 4;
            _count = 0;
            _buckets = new Bucket<TKey, TValue>[_capacity];
        }
        private int GetBucketIndex(TKey key, int count)
        {
            if (_comparer?.Equals(key, default) ?? Equals(key, default))
                throw new ArgumentNullException(nameof(key));

            int Hash = _comparer?.GetHashCode(key) ?? key.GetHashCode();

            return Math.Abs(Hash) % count;

        }
        private void ArrayResize()
        {
            if (_count != _capacity)
                return;

            _capacity *= 2;
            var newBuckets = new Bucket<TKey, TValue>[_capacity];
            Array.Copy(_buckets, newBuckets, _buckets.Length);
            _buckets = newBuckets;
        }
        public void Add(Bucket<TKey, TValue> item)
        {
             var key = item.Pair.Key;
             var value = item.Pair.Value; 

            Add(key, value);
        }

        public void Clear()
        {
            _buckets = null;
            _count = 0;
            _capacity = 0;
        }

        public bool Contains(Bucket<TKey, TValue> item)
        {
            var key = item.Pair.Key;
            var value = item.Pair.Value;

            var index = GetBucketIndex(key, _buckets.Length);
            if (_buckets[index].IsOccupied)
            {
                if (_comparer?.Equals(key, _buckets[index].Pair.Key) 
                    ?? Equals(key, _buckets[index].Pair.Key)
                    && Equals(value, _buckets[index].Pair.Value))
                    return true;
            }
            return false;
        }

        public void CopyTo(Bucket<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (array.Length - arrayIndex < _buckets.Length)
                throw new ArgumentException("The destination array does not have enough space.");

            Array.Copy(_buckets, 0, array, arrayIndex, _buckets.Length);

        }


        public bool Remove(Bucket<TKey, TValue> item)
        {
            var key = item.Pair.Key;
            var value = item.Pair.Value;

            var index = GetBucketIndex(key, _buckets.Length);
            if (_buckets[index].IsOccupied)
            {
                if (_comparer?.Equals(key, _buckets[index].Pair.Key)
                    ?? Equals(key, _buckets[index].Pair.Key)
                    && Equals(value, _buckets[index].Pair.Value))
                {
                    _buckets[index] = default(Bucket<TKey, TValue>);
                    return true;
                }
            }
            return false;
        }

        public IEnumerator<Bucket<TKey, TValue>> GetEnumerator()
        {
            return new CustomEnumeraot<TKey,TValue>(_buckets, _count);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(TKey key, TValue value)
        {
            if (_comparer?.Equals(key, default(TKey)) ?? Equals(key, default(TKey)))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var index = GetBucketIndex(key, _buckets.Length);
            var originalindex = index;
            do
            {

                if (_buckets[index].IsOccupied)
                {
                    if (_comparer?.Equals(_buckets[index].Pair.Key, key) ?? Equals(key, _buckets[index].Pair.Key))
                        throw new ArgumentException($"the key {key} is Already Exist");
    
                    index = (index + 1) % _buckets.Length;
                }
                else
                {
                    SetPair(key, value, index);
                    return;
                }
                if(index == originalindex)
                {
                    ArrayResize();
                    originalindex = _buckets.Length;
                }
            } while (true);


        }
        public void SetPair(TKey key, TValue value,int index)
        {
            Bucket<TKey, TValue> bucket = new Bucket<TKey, TValue>();

            bucket.Pair = new KeyValuePair<TKey, TValue>(key, value);
            bucket.IsOccupied = true;
            _buckets[index] = bucket;
            _count++;
        }

        public bool ContainsKey(TKey key)
        {
            int index = GetBucketIndex(key,_buckets.Length);

            if (_comparer?.Equals(key, _buckets[index].Pair.Key)
                ?? Equals(key, _buckets[index].Pair.Key)
                && _buckets[index].IsOccupied)
            {
                return true;
            }
            return false;
        }

        public bool Remove(TKey key)
        {
            int index = GetBucketIndex(key, _buckets.Length);
            if (_comparer?.Equals(key, _buckets[index].Pair.Key) ??
                Equals(key, _buckets[index].Pair.Key))
            {
                _buckets[index] = default(Bucket<TKey, TValue>);
                _count--;
                return true;
            }
            return false;
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            var index = GetBucketIndex(key, _buckets.Length);
            value = default(TValue);
            if (_comparer?.Equals(key, _buckets[index].Pair.Key) ??
                Equals(key, _buckets[index].Pair.Key) && _buckets[index].IsOccupied)
            {
                value = _buckets[index].Pair.Value;
                return true;
            }
            return false;
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Bucket<TKey,TValue> bucket = new Bucket<TKey,TValue>();
            bucket.IsOccupied = true;
            bucket.Pair = item;

            Add(item.Key,item.Value);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            var bucket = new Bucket<TKey,TValue>();
            return Contains(bucket);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotFiniteNumberException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            Bucket<TKey, TValue> bucket = new Bucket<TKey, TValue>();
            bucket.Pair = item;
            bucket.IsOccupied = true;

            return Remove(bucket);
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            foreach (var bucket in _buckets)
            {
                if (bucket.IsOccupied)
                    yield return bucket.Pair;
            }
        }

        public void CopyTo(Array array, int index)
        {
            if (array is null) throw new ArgumentNullException(nameof(array));
            if (array.Length - index < Count) throw new ArgumentException("Insufficient space in target array.");

            foreach (var bucket in _buckets)
            {
                if (bucket.IsOccupied)
                {
                    array.SetValue(bucket.Pair, index++);
                }
            }
        }
    }

    public class CustomEnumeraot<TKey, TValue> : IEnumerator<Bucket<TKey, TValue>>
    {
        private Bucket<TKey, TValue>[] _buckets;
        private Bucket<TKey, TValue> _current;
        private int _count;
        private int _index;
        public CustomEnumeraot(Bucket<TKey, TValue>[] buckets, int count)
        {
            _buckets = buckets;
            _count = _buckets.Length;
        }

        public Bucket<TKey, TValue> Current => _current;

        object IEnumerator.Current => _current;

        public void Dispose()
        {

        }

        public bool MoveNext()
        {
            while (_index < _count)
            {
                if (_buckets[_index].IsOccupied)
                {
                    _current = _buckets[_index];
                    _index++;
                    return true;
                }
                _index++;
            }
            return false;
        }

        public void Reset()
        {

        }
    }

}
