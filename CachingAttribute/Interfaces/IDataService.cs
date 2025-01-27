using CachingAttribute.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CachingAttribute.Interfaces
{
    public interface IDataService
    {
        [CacheResult]
         int GetData(int Id);
    }
}
