using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomThreadPool.Enums
{
    public enum ThreadState
    {
        Sleep = 0,
        Wait = 1,
        Working = 2,
    }
}
