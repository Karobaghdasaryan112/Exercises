using System.Collections;
namespace CustomLinkedList.Services
{
    public class CustomLinkedList<T> : ICollection<T>, IReadOnlyCollection<T>, IEnumerable<T>
    {
        public CustomLinkedListNode<T>? _first { get; set; }
        private CustomLinkedListNode<T>? _last { get; set; }


        private int _count = 0;
        public CustomLinkedList()
        {

        }
        public CustomLinkedList(IEnumerable<T> Collection)
        {
            foreach (var item in Collection)
            {
                AddLast(item);
            }
        }

        public int Count => _count;

        public bool IsReadOnly => false;



        public void Clear()
        {
            _first = null;
            _last = null;
            _count = 0;
        }

        public bool Contains(T item)
        {
            if (item == null)
                throw new ArgumentNullException("item can not be null");

            var Node = _first;
            while (Node != null)
            {
                if (item.Equals(Node.value))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator() => new CustomEnumerator<T>(_first);

        public bool Remove(T item)
        {
            if (item == null)
                throw new ArgumentNullException("item cannot be null!");
            if (_count == 0)
                return false;

            var Node = _first;
            while (Node != null)
            {
                if (item.Equals(Node.value))
                {
                    if (Node == _first)
                    {
                        _first = _first._next;
                    }
                    else if (Node == _last)
                    {
                        _last = _last._prev;
                    }

                    Node._prev._next = Node._next;
                    Node._next._prev = Node._prev;
                    Node = null;
                    _count--;
                    break;
                }
                Node = Node._next;
            }
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(T item)
        {
            AddLast(item);
        }
        public void AddFirst(T value)
        {
            if (value == null) throw new ArgumentNullException("value can not be null");

            if (_first == null)
            {
                _first = new CustomLinkedListNode<T>
                {
                    value = value,
                    _prev = null,
                    _next = null
                };
                _last = _first;
            }
            else
            {
                _first._prev = new CustomLinkedListNode<T>
                {
                    value = value,
                    _next = _first,
                    _prev = null
                };
                _first = _first._prev;
            }
            _count++;
        }

        public void AddLast(T value)
        {
            if (value == null) throw new ArgumentNullException("value can not be null");

            if (_last == null)
            {
                AddFirst(value);
            }
            else
            {
                _last._next = new CustomLinkedListNode<T>
                {
                    value = value,
                    _prev = _last,
                    _next = null
                };
                _last = _last._next;

            }
            _count++;
        }
        public void AddBefore(CustomLinkedListNode<T> node, T value)
        {
            if (node == null) throw new ArgumentNullException("Node cannot be null");
            if (value == null) throw new ArgumentNullException("value cannot be null");

            var current = _first;

            while (current != null)
            {
                if (current.value.Equals(node.value))
                {
                    var NewNode = new CustomLinkedListNode<T>
                    {
                        value = value,
                        _next = node,
                        _prev = node._prev
                    };

                    if (node._prev != null)
                    {
                        node._prev._next = NewNode;
                    }
                    else
                    {
                        _first = NewNode;
                    }

                    node._prev = NewNode;
                    _count++;
                    return;
                }
                current = current._next;

            }
        }
        public void AddAfter(CustomLinkedListNode<T> node, T value)
        {
            if (node == null) throw new ArgumentNullException("Node cannot be null");
            if (value == null) throw new ArgumentNullException("value cannot be null");

            var current = _first;

            while (current != null)
            {
                if (current.Equals(node))
                {
                    var newNode = new CustomLinkedListNode<T>
                    {
                        value = value,
                        _next = node._next,
                        _prev = node
                    };

                    if (node._next != null)
                    {
                        node._next._prev = newNode;
                    }
                    else
                    {
                        _last = newNode;
                    }

                    node._next = newNode;

                    _count++;
                    return;
                }

                current = current._next;
            }
            throw new ArgumentException("Node not found in the list.");
        }


        public void RemoveFirst()
        {
            if (_first == null)
                throw new InvalidOperationException("The list is empty.");

            if (_first == _last)
            {
                _first = _last = null;
            }
            _first = _first._next;
            _first._prev = null;
            _count--;
        }
        public void RemoveLast()
        {
            if (_last == null)
                throw new InvalidOperationException("The list is empty.");

            if (_first == _last)
            {
                _first = _last = null;
            }
            _last = _last._prev;
            _last._next = null;
            _count--;
        }
    }
    public class CustomEnumerator<T> : IEnumerator<T>
    {
        private T _value;   
        private CustomLinkedListNode<T>? _first;
        public CustomEnumerator(CustomLinkedListNode<T> first)
        {
            _first = first;
        }

        public T Current => _value;

        object IEnumerator.Current => _value;

        public void Dispose()
        {
            _first = null;
        }

        public bool MoveNext()
        {
            while (_first != null)
            {
                _value = _first.value;
                _first = _first._next;
                return true;
            }
            return false;
        }

        public void Reset()
        {
        }
    }
}
