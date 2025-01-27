using ProxcyService.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxcyService.Services
{
    public interface IService
    {
        [LoggingExecutionAttribute]
        void DoWork();
        [LoggingExecutionAttribute]
        void DoOtherWor();
    }
}
