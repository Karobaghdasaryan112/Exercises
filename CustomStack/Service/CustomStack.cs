using System.Collections;

namespace CustomStack.Service
{
    public class CustomStack<T> : 
        IEnumerable<T>, 
        ICollection,
        IReadOnlyCollection<T>
    {
        private T[]? _array;
        private int _count;
        private int _capacity;
        public CustomStack()
        {
            Initialize();
        }
        public CustomStack(IEnumerable<T> array)
        {
            _count = array.Count();
            _capacity = _count * 2;
            _array = new T[_capacity];

            int index = 0;
            foreach (var item in array)
            {
                _array[index++] = item;
            }

        }
        public CustomStack(int capacity)
        {
            _capacity = capacity;
            _count = 0;
            _array = new T[_capacity];
        }
        private void Initialize()
        {
            _count = 0;
            _capacity = 4;
            _array = new T[_capacity];

        }
        private void ArrayResize()
        {
            _capacity *= 2;
            T[] newArray = new T[_capacity];
            Array.Copy(_array, newArray, _count);
            _array = newArray;
        }

        public int Count => _count;

        private readonly object _syncRoot = new object();
        public object SyncRoot => _syncRoot;

        public bool IsSynchronized => false;

        public IEnumerator<T> GetEnumerator()
        {
            return new CustomEnumerator<T>(_array, _count);
        }

        public void Push(T item)
        {
            if (_count == _capacity)
                ArrayResize();

            _array[_count] = item;
            _count++;
        }
        public T Pop()
        {
            if (_count == 0)
                throw new InvalidOperationException("there is no element");
            T value = _array[_count - 1];
            _array[_count - 1] = default(T);
            _count--;
            return value;
        }

        public T Peek()
        {
            if (_count == 0)
                throw new InvalidOperationException("there is no element");

            return _array[_count - 1];
        }

        public bool TryPeek(out T value)
        {
            value = default;

            if (_count == 0)
                return false;

            value = _array[_count - 1];
            return true;
        }
        public bool TryPop(out T value)
        {
            value = default;

            if (_count == 0)
                return false;

            value = _array[_count - 1];

            _array[_count - 1] = default;
            _count--;

            return true;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (index < 0 || index >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (array.Length - index < _count)
                throw new ArgumentException("The destination array has insufficient space.");

            Array.Copy(_array, 0, array, index, _count);
        }
    }
    public class CustomEnumerator<T> : IEnumerator<T>
    {
        private T[] _array;
        private int _count;
        private int _index = 0;
        private T _current;
        public CustomEnumerator(T[] array, int count)
        {
            _array = array;
            _count = count;
        }

        public T Current => _current;

        object IEnumerator.Current => _current;

        public void Dispose()
        {
            //No Resource Disposing
        }

        public bool MoveNext()
        {
            while(_index < _count)
            {
                _current = _array[_index];
                _index++;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            _count = -1;
            _index = -1;
            _array = default(T[]);
        }
    }
}
