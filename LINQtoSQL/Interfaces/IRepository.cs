using LINQtoSQL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQtoSQL.Interfaces
{
    public interface IRepository<T>
    {
        List<T> Entities { get; }
    }
}
