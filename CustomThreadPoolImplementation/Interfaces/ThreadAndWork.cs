using CustomThreadPoolImplementation.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomThreadPoolImplementation.Interfaces
{
    public class ThreadAndWork
    {
        public Thread Thread { get; set; }
        public CustomThreadState State { get; set; }
        public ThreadAndWork(Thread thread, CustomThreadState State)
        {
            Thread = thread;
            this.State = State;
        }
        public ThreadAndWork()
        {

        }
    }
}
