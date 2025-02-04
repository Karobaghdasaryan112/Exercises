using CustomThreadPoolImplementation.Interfaces;

namespace CustomThreadPoolImplementation
{
    /// <summary>
    /// Custom Implementation ThreadPool
    /// Methods
    /// QueueUserWorkItem;
    /// 
    /// </summary>
    public class CustomThreadPool : IThreadPool
    {
        //Private Fields
        private int _minimumThreadsCount;
        private int _maximumThreadsCount;
        private int _activeThreadsCount;
        private bool _isOptimized = false;
        private List<ThreadAndWork> threads;
        private Queue<WaitCallback> queue;
        private object _lock = new object();
        private int _enqueueMethods = 0;
        //Public Fields
        public int PendingWorkItemCount {  get; private set; }
        public int CompletedWorkItemCount {  get; private set; }
        public int ActiveThreadsCount => _activeThreadsCount;
        public CustomThreadPool()
        {
            Initialize();
        }
        private void Initialize()
        {
            InitializeMinAndMaxWorkerThreadsCount();
            //Initialize Threads And Queue 
            threads = new List<ThreadAndWork>(_minimumThreadsCount);
            queue = new Queue<WaitCallback>();

            for (int ThreadIndex = 0; ThreadIndex < _minimumThreadsCount; ThreadIndex++)
            {
                Thread newThread = new Thread(ThreadWorker)
                {
                    IsBackground = true,
                };

                newThread.Name = $"thread: {ThreadIndex}";
                var ThreadAndWork = new ThreadAndWork(newThread, Enums.CustomThreadState.sleep);
                threads.Add(ThreadAndWork);
                _activeThreadsCount++;
                newThread.Start(ThreadIndex);
            }

        }

        //Initialize Min And Max Threads Count When Methods for Changing Count for Those Don't Call
        private void InitializeMinAndMaxWorkerThreadsCount()
        {
            if (_minimumThreadsCount == 0)
                _minimumThreadsCount = 4;

            if (_maximumThreadsCount == 0)
                _maximumThreadsCount = 8;
        }

        public void ThreadWorker(object state)
        {
            while (true)
            {
                int index = (int)state;

                WaitCallback waitCallback = null;
                lock (_lock)
                {
                    while (queue.Count == 0)
                    {
                        threads[index].State = Enums.CustomThreadState.Wait;
                        Monitor.Wait(_lock);
                    }
                    queue.TryDequeue(out var result);
                    if (result != null)
                    {
                        waitCallback = result;
                        PendingWorkItemCount--;
                    }

                    threads[index].State = Enums.CustomThreadState.Working;
                }

                waitCallback?.Invoke(state);

                if(queue.Count == 0 || threads.Where(thread => thread.State == Enums.CustomThreadState.Wait || thread.State == Enums.CustomThreadState.Working).Count() == threads.Count() && !_isOptimized)
                {
                    Thread thread = new Thread(Optimization);
                    thread.IsBackground = true;
                    thread.Start();
                    _isOptimized = true;
                }
                lock (_lock)
                {

                    CompletedWorkItemCount++;
                    _activeThreadsCount--;
                    Console.WriteLine("Active Threads Count" + _activeThreadsCount);
                }
            }
        }

        public void GetMaxThreads(out int workerThreads, out int completionPortThreads)
        {
            //Not Supported
            completionPortThreads = 0;

            workerThreads = _maximumThreadsCount;
        }

        public void GetMinThreads(out int workerThreads, out int completionPortThreads)
        {
            //Not Supported
            completionPortThreads = 0;

            workerThreads = _minimumThreadsCount;
        }

        public bool QueueUserWorkItem(WaitCallback callback) => QueueUserWorkItem(callback, null);


        public bool QueueUserWorkItem(WaitCallback callback, object? state)
        {
            if (callback == null)
                throw new ArgumentNullException(nameof(WaitCallback), "CallBack Function Cannot be Null here!..");
            lock (_lock)
            {

                queue.Enqueue(callback);
                PendingWorkItemCount++;

                Monitor.PulseAll(_lock);
            }
            return true;
        }
        //Conditions

        //AllThreads Is Activly And DoingWork But there is Item in Queue


        //if queue count are more than workerThreadsCount (optimization) deleting  Threads that dont doing any work


        //if working take  more time we can be repeat it after few time letter and if it doesnt work we shuld be deleting this item from queue

        private void Optimization()
        {
            if (CreatingHelperThreadCondition())
            {
                Thread.Sleep(300);

                if (CreatingHelperThreadCondition())
                {
                    Thread newThread = new Thread(ThreadWorker)
                    {
                        IsBackground = true
                    };
                    var ThreadAndWork = new ThreadAndWork(newThread, Enums.CustomThreadState.sleep);
                    threads.Add(ThreadAndWork);
                    _activeThreadsCount++;
                    newThread.Start(threads.Count - 1);
                }
            }
            if (DeletingHelperThreadCondition())
            {
                Thread.Sleep(300);
                if (DeletingHelperThreadCondition())
                {
                    var inactiveThreads = threads.Where(thread => thread.State == Enums.CustomThreadState.Wait && !queue.Any()).ToList();
                    if (inactiveThreads.Any())
                    {
                        threads.RemoveAll(thread => thread.State == Enums.CustomThreadState.Wait);
                        _activeThreadsCount = threads.Count;
                    }
                }
            }
        }
        //Conditions
        private bool CreatingHelperThreadCondition()
        {
                if (threads.Where(thread => thread.State == Enums.CustomThreadState.Working).Count() == threads.Count && _activeThreadsCount < _maximumThreadsCount && queue.Any())
                    return true;

            return false;
        }
        private bool DeletingHelperThreadCondition()
        {
            if (_activeThreadsCount > PendingWorkItemCount  && !queue.Any())
                return true;
            return false;
        }


        public bool QueueUserWorkItem<TState>(Action<TState> action, TState state, bool preferLocal)
        {
            throw new NotImplementedException();
        }

        public bool SetMaxThreads(int workerThreads, int completionPortThreads)
        {
            if (SetWorkerThreadsCondition(workerThreads, "Maximum worker Threads"))
                _maximumThreadsCount = workerThreads;

            return true;
        }

        public bool SetMinThreads(int workerThreads, int completionPortThreads)
        {

            if (SetWorkerThreadsCondition(workerThreads, "Minimum worker Threads"))
                _minimumThreadsCount = workerThreads;

            return true;
        }

        public bool SetWorkerThreadsCondition(int MinOrMaxWorkerThreads,string WorkerThreads)
        {
            if (MinOrMaxWorkerThreads <= 0)
                throw new ArgumentException($"{WorkerThreads} must be greather than 0", MinOrMaxWorkerThreads.ToString());

            if (_minimumThreadsCount >= MinOrMaxWorkerThreads)
                throw new ArgumentException($"{WorkerThreads} must be greather than Minimum Worker Threads", MinOrMaxWorkerThreads.ToString());

            return true;
        }
    }
}
