using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enumerable.Enumerators
{
    public class CustomEnumerator<T> : IEnumerator<T>
    {
        private List<T> _list;
        private T _current;
        private int _index;
        public CustomEnumerator(List<T> list)
        {
            _list = list;
        }

        public T Current => _current;

        object IEnumerator.Current => _current;

        public void Dispose()
        {
            //no resource to dispose
        }

        public bool MoveNext()
        {
            while (_index < _list.Count)
            {
                _current = _list[_index];
                _index++;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            _current = default(T);
            _index = 0;
        }
    }
}
