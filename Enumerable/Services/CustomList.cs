
using Enumerable.Enumerators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enumerable.Services
{

    public class CustomList<T> : IEnumerable<T>
    {
        private List<T> _list;
        public CustomList()
        {
            _list = new List<T>();
        }
        public void AddToList(T Value)
        {
            _list.Add(Value);
        }
        public IEnumerator<T> GetEnumerator() => new CustomEnumerator<T>(_list);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

      
    }
}
