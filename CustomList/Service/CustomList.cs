using System.Collections;
namespace CustomList.Service
{

    //Inheritance Diagram
    //
    //|--List<T>
    //   |--IList<T>
    //      |--ICollection<T>
    //      |  |--IEnumerable<T>
    //      |--IEnumerable   
    //   |--IReadOnlyList<T>
    //      |--IReadOnlyCollection<T>
    //   |--IList
    //      |--ICollection
    //         |-IEnumerable
    //  

    public class CustomList<T> : IList<T>, IReadOnlyList<T>
    {
        private T[] _values {  get; set; }
        private int _capacity {  get; set; }
        private int _count {  get; set; }
        private void Initialize()
        {
            _capacity = 4;
            _values = new T[_capacity];
            _count = 0;
        }
            
        public CustomList(int capacity)
        {
            _capacity = capacity;
            _values = new T[capacity];
            _count = 0;
        }
        public CustomList(IEnumerable<T> Values) 
        {
            _count = Values.Count();
            _capacity = _count * 2;
            _values = new T[_capacity];

            int index = 0;
            foreach (var Value in Values)
            {
                _values[index] = Value;
                index++;
            }
        }
        public CustomList()
        {
            Initialize();
        }


        public T this[int index]
        {
            get
            {
                if (index < 0 || index > Count)
                    throw new IndexOutOfRangeException($"Index {index} is out of range. Valid range is [0, {Count - 1}].");

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                return _values[index];
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }

            set
            {
                if (index < 0 || index > Count)
                    throw new IndexOutOfRangeException($"Index {index} is out of range. Valid range is [0, {Count - 1}].");

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                _values[index] = value;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
        }

        public int Count => _count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            if (Count == _capacity)
            {
                CustomArrayResize();
            }

            _values[Count] = item;
            _count++;
        }
        private void CustomArrayResize()
        {
            _capacity *= 2;
            T[] newvalues = new T[_capacity];
            Array.Copy(_values, newvalues, Count);
            _values = newvalues;
        }
        public void Clear()
        {
            Initialize();
        }

        public bool Contains(T item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            var ContainsItem = _values.Where(value => value.Equals(item)).FirstOrDefault();

            return ContainsItem == null ? false : true;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(array, _values, Count);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new CustomEnumerator<T>(_count, _values);
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < Count; i++)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                if (_values[i].Equals(item))
                {
                    return i;
                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException($"Index {index} is out of range. Valid range is [0, {Count - 1}].");

            T[] greatherThanIndexArray = new T[Count - index];
            Array.Copy(_values, index, greatherThanIndexArray, 0, Count - index);

            if (Count == _capacity)
                CustomArrayResize();

            _values[index] = item;

            Array.Copy(greatherThanIndexArray, 0, _values, index + 1, Count - index);

            _count++;
        }

        public bool Remove(T item)
        {
            if (!_values.Contains(item))
                return false;
            else
            {

                for (int i = 0; i < Count; i++)
                {
                    if (item != null)
                    {
                        if (item.Equals(_values[i]))
                        {
                            Array.Copy(_values, i + 1, _values, i, Count - i - 1);
                            _values[Count - 1] = default(T);
                            _count--;

                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public void RemoveAt(int index)
        {

            if (index < 0 || index >= Count)
                throw new IndexOutOfRangeException($"Index {index} is out of range. Valid range is [0, {Count - 1}].");

            Array.Copy(_values, index + 1, _values, index, Count - index - 1);
            _values[Count - 1] = default(T);
            _count--;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator(); 
    }
    public class CustomEnumerator<T> : IEnumerator<T>
    {
        private T[]? _values;
        private int _index;
        private int _count;
        private T? _current;
        public CustomEnumerator(int Count, T[] values)
        {
            _values = values;
            _count = Count;
        }

        public T Current => _current;

        object IEnumerator.Current => _current;

        public void Dispose()
        {
            _values = default;
        }

        public bool MoveNext()
        {
            while(_count  > _index)
            {
                _current = _values[_index];
                ++_index;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            _index = -1;
            _current = default;
        }
    }

}
