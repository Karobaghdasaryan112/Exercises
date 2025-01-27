using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxcyService.Attributes
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple =false)]
    public class LoggingExecutionAttribute : Attribute
    {

    }
}
