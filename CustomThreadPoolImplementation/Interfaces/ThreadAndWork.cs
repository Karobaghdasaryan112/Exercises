using System;
namespace CustomThreadPool
{
    public class ThreadAndState
    {
        public Thread? Thread { get; set; }
        public Enums.ThreadState ThreadState { get; set; }

        public ThreadAndState(Thread thread, Enums.ThreadState threadState)
        {
            Thread = thread;
            ThreadState = threadState;
        }


    }
}
