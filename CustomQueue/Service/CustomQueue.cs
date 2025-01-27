using System.Collections;
using System.Diagnostics;
namespace CustomQueue.Service
{
    public class CustomQueue<T> : IReadOnlyCollection<T>
    {
        private T[]? _array;
        private int _head;
        private int _tail;
        private int _size;
        private int _capacity;
        public CustomQueue(int capacity)
        {
            _size = 0;
            _capacity = capacity;
            _array = new T[capacity];
        }
        public CustomQueue()
        {
            Initialize();
        }

        public CustomQueue(IEnumerable<T> values)
        {
            _size = values.Count();
            _capacity = _size * 2;
            _array = new T[_size];

            foreach (T value in values)
            {
                _array[_head++] = value;
            }
        }

        private void Initialize()
        {
            _capacity = 4;
            _size = 0;
            _array = new T[_capacity];
        }
        private void Resize()
        {
            T[] newArray = new T[_capacity * 2];
            Array.Copy(_array, newArray, _size);
            _array = newArray;
        }
        public void Enqueue(T value)
        {
            if (_size == _capacity)
                Resize();
            if (_tail > _head)
                return;
            _array[_head] = value;
            _head++;
            _size++;

        }

        public bool TryDequeue(out T value)
        {
            value = default;
            if (_tail >= _head)
                return false;

            value = _array[_tail];
            _array[_tail] = default;
            _tail++;
            return true;
        }

        public int Count => _size;

        public IEnumerator<T> GetEnumerator()
        {
            return new CustomEnumerator<T>(_array, _size);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


    }
    public class CustomEnumerator<T> : IEnumerator<T>
    {
        private T[] _array;
        private int _tail;
        private int _head;
        private int _size;
        private T _current;
        public CustomEnumerator(T[] array,int size)
        {
            _array = array;
            _tail = 0;
            _size = size;
            _head = _size - 1;
        }

        public T Current => _current;

        object IEnumerator.Current => _current;

        public void Dispose()
        {
            _array = default; 
            _tail = 0;
            _size = 0;
            _head = 0;
        }

        public bool MoveNext()
        {

            while (_tail <= _head)
            {
                _current = _array[_tail];
                _tail++;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            _head = 0;
            _tail = 0;
            _size = 0;
            _array = new T[4];
        }
    }
}
