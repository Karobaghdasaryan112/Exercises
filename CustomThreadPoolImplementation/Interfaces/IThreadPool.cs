using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomThreadPoolImplementation.Interfaces
{
    public interface IThreadPool
    {

        //QueueUserWorkItem
        bool QueueUserWorkItem(WaitCallback callback);

        bool QueueUserWorkItem(WaitCallback callback, object state);

        bool QueueUserWorkItem<TState>(Action<TState> action,TState state,bool preferLocal);


        //GetMinMaxThreadsAndPortcompletion
        void GetMinThreads(out int workerThreads, out int completionPortThreads);

        void GetMaxThreads(out int workerThreads, out int completionPortThreads);


        //SetMinMaxThreadsAndPortcompletion
        bool SetMinThreads(int workerThreads, int completionPortThreads);

        bool SetMaxThreads(int workerThreads, int completionPortThreads);


        
    }
}
