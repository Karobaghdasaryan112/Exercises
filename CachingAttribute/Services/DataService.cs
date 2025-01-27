using CachingAttribute.Attributes;
using CachingAttribute.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CachingAttribute.CacheData
{
    public class DataService : IDataService
    {
        public int GetData(int Id)
        {
            Console.WriteLine("doing a Work");
            Thread.Sleep(1000);
            return Id * 10;
        }
    }
}
