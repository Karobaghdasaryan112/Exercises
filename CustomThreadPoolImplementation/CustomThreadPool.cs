using System;

namespace CustomThreadPool
{
    public class ThreadPool
    {
        //private Fields

        private int _minimumThreadsCount;

        private int _maximumThreadsCount;

        private Timer? _optimizationTimer;

        private Queue<WaitCallback>? _workQueue;

        private List<ThreadAndState>? _threads;

        private Thread _optimizedThread;

        private object _lock = new object();

        private int _activeThreadsCount;


        //Public fields
        // Public Properties
        /// <summary>
        /// Gets the number of pending work items in the queue.
        /// </summary>
        public int PendingWorkItemCount { get; private set; }

        /// <summary>
        /// Gets the number of completed work items.
        /// </summary>
        public int CompletedWorkItemCount { get; private set; }

        /// <summary>
        /// Gets the number of currently active threads.
        /// </summary>
        public int ActiveThreadsCount => _activeThreadsCount;


        //Constants

        private const int MIN_DEFAULT_THREADS_COUNT = 4;

        private const int MAX_DEFAULT_THREADS_COUNT = 8;


        //Ctors

        // Constructors
        /// <summary>
        /// Initializes a thread pool with a specified number of minimum and maximum threads.
        /// </summary>
        public ThreadPool(int minimumThreadsCount, int maximumThreadsCount)
        {
            ThreadsCountException(minimumThreadsCount, maximumThreadsCount);

            _maximumThreadsCount = maximumThreadsCount;
            _minimumThreadsCount = minimumThreadsCount;

            Initialize();
        }


        /// <summary>
        /// Initializes a thread pool with a given queue of work items.
        /// </summary>
        public ThreadPool(Queue<WaitCallback> workQueue)
        {
            Initialize();
            _workQueue = workQueue;

            foreach (var item in workQueue)
            {
                QueueUserWorkItem(item);
            }
        }

        /// <summary>
        /// Initializes a thread pool with default settings.
        /// </summary>
        public ThreadPool()
        {
            Initialize();
        }


        //Methods

        /// <summary>
        /// Initializes internal data structures and starts worker threads.
        /// </summary>
        private void Initialize()
        {
            _workQueue ??= new Queue<WaitCallback>();
            _threads = new List<ThreadAndState>();
            _optimizedThread = new Thread(Optimization)
            {
                IsBackground = true,
            };

            _activeThreadsCount = 0;


            if (_minimumThreadsCount == 0)
                _minimumThreadsCount = MIN_DEFAULT_THREADS_COUNT;

            if (_maximumThreadsCount == 0)
                _maximumThreadsCount = MAX_DEFAULT_THREADS_COUNT;



            for (int ThreadIndex = 0; ThreadIndex < _minimumThreadsCount; ThreadIndex++)
            {
                Thread newThread = new Thread(ThreadWorker)
                {
                    IsBackground = true
                };
                newThread.Name = $"Thread: {ThreadIndex}";
                Enums.ThreadState state = Enums.ThreadState.Sleep;
                ThreadAndState newThreadAndState = new ThreadAndState(newThread, state);

                _threads.Add(newThreadAndState);
                newThread.Start(ThreadIndex);
                _activeThreadsCount++;
            }

            _optimizationTimer = new Timer(Optimization, null, 2000, 2000);

        }


        /// <summary>
        /// Worker thread method that processes work items from the queue.
        /// </summary>
        private void ThreadWorker(object state)
        {
            var ThreadAndState = _threads?.FindThreadAndState(Thread.CurrentThread.Name);

            Console.WriteLine($"{Thread.CurrentThread.Name} is working");

            while (true)
            {
                WaitCallback waitCallback = null;

                lock (_lock)
                {
                    while (_workQueue.Count == 0)
                    {

                        ThreadAndState.ThreadState = Enums.ThreadState.Wait;
                        Monitor.Wait(_lock);
                    }
                    waitCallback = _workQueue.Dequeue();
                    ThreadAndState.ThreadState = Enums.ThreadState.Working;
                    _activeThreadsCount++;

                }
                try
                {
                    waitCallback?.Invoke(state);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"exception was thrwed when try to invoke some work by {Thread.CurrentThread.Name}");

                    throw;
                }
                finally
                {
                    CompletedWorkItemCount++;
                    PendingWorkItemCount--;
                    _activeThreadsCount--;
                }
            }
        }

        /// <summary>
        /// Checks and adjusts the number of threads dynamically.
        /// </summary>
        private void Optimization(object? state)
        {
            //DeleginThreads Optimization
            if (DeletingHelperThreadsCondition())
                DeletingThreasds();

            //AddingThreads Optimization
            if (AddingHelperThreadsCondition())
                AddingThreads();

            //CancelWork Optimization
            if (CancelingHerlperCondition())
                Cancelingwork();
        }


        /// <summary>
        /// Adds a work item to the queue and notifies waiting threads.
        /// </summary>
        public void QueueUserWorkItem(WaitCallback callback)
        {
            if (callback == null)
                throw new ArgumentNullException($"{nameof(callback)}", "callback function is null here");

            lock (_lock)
            {
                _workQueue?.Enqueue(callback);
                PendingWorkItemCount++;
                Monitor.PulseAll(_lock);
            }
        }


        //DeleginThreads Optimization
        /// <summary>
        /// Checks if threads should be removed.
        /// </summary>
        private bool DeletingHelperThreadsCondition()
        {
            if (_workQueue?.Count == 0 &&
                _threads?.FindWorkingStateThreads().Count < _threads?.Count &&
                _threads.Count > _minimumThreadsCount)
                return true;
            return false;
        }

        private void DeletingThreasds()
        {
            var WaitingThreads = _threads?.FindWaitingStateThreads();

            if (WaitingThreads != null)
            {
                lock (_lock)
                {
                    int DeletingThreadsCount = 0;

                    _threads?.RemoveWaitingThreads(ThreadAndState =>
                    (ThreadAndState.ThreadState == Enums.ThreadState.Wait
                    && _threads.Count >= _minimumThreadsCount), out DeletingThreadsCount);

                    _activeThreadsCount -= DeletingThreadsCount;
                }
            }
        }


        //AddingThreads Optimization
        private bool AddingHelperThreadsCondition()
        {
            if (_workQueue?.Count > 0 &&
                _threads?.FindWorkingStateThreads().Count == _threads?.Count &&
                _threads?.Count < _maximumThreadsCount)
            {
                return true;
            }

            return false;
        }

        private void AddingThreads()
        {

            Thread thread = new Thread(ThreadWorker);
            Enums.ThreadState threadState = Enums.ThreadState.Sleep;

            ThreadAndState threadAndState = new ThreadAndState(thread, threadState);

            thread.Name = $"Thread: {_threads?.Count}";

            _threads?.Add(threadAndState);

            _activeThreadsCount++;

            thread.Start();
        }

        //CancelWork Optimization
        private bool CancelingHerlperCondition()
        {
            return true;
        }

        private void Cancelingwork()
        {

        }

        //Exceptions
        /// <summary>
        /// Validates the thread count parameters.
        /// </summary>
        private void ThreadsCountException(int minimumThreadsCount, int maximumThreadsCount)
        {
            if (minimumThreadsCount <= 0 || maximumThreadsCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(minimumThreadsCount),
                    "Min or Max Threads Count cannot be less than 1");

            if (minimumThreadsCount >= maximumThreadsCount)
                throw new ArgumentOutOfRangeException(nameof(minimumThreadsCount),
                    "Minimum count of threads cannot be greater than maximum count");
        }

    }

    public static class ListUtils
    {
        /// Finds a ThreadAndState object by thread name.
        /// </summary>
        /// <param name="threadAndStates">The list of ThreadAndState objects.</param>
        /// <param name="nameofThread">The name of the thread to find.</param>
        /// <returns>The found ThreadAndState object.</returns>
        /// <exception cref="Exception">Thrown if no thread with the specified name is found.</exception>
        public static ThreadAndState FindThreadAndState(this List<ThreadAndState> threadAndStates, string nameofThread)
        {
            var ThreadAndState = threadAndStates.Where(lookingThreadAndSate => lookingThreadAndSate?.Thread?.Name == nameofThread).FirstOrDefault();

            if (ThreadAndState == null)
                throw new Exception($"thread By name {nameofThread} isnt find");

            return ThreadAndState;
        }
        /// <summary>
        /// Finds all threads that are in the Working state.
        /// </summary>
        /// <param name="threadAndStates">The list of ThreadAndState objects.</param>
        /// <returns>A list of ThreadAndState objects in the Working state.</returns>
        public static List<ThreadAndState> FindWorkingStateThreads(this List<ThreadAndState> threadAndStates)
        {
            return threadAndStates.Where(ThreadAndState => ThreadAndState.ThreadState == Enums.ThreadState.Working).ToList();
        }

        /// <summary>
        /// Finds all threads that are in the Sleeping state.
        /// </summary>
        /// <param name="threadAndStates">The list of ThreadAndState objects.</param>
        /// <returns>A list of ThreadAndState objects in the Sleeping state.</returns>
        public static List<ThreadAndState> FindSleepingStateThreads(this List<ThreadAndState> threadAndStates)
        {
            return threadAndStates.Where(ThreadAndState => ThreadAndState.ThreadState == Enums.ThreadState.Sleep).ToList();
        }

        /// <summary>
        /// Finds all threads that are in the Waiting state.
        /// </summary>
        /// <param name="threadAndStates">The list of ThreadAndState objects.</param>
        /// <returns>A list of ThreadAndState objects in the Waiting state.</returns>
        public static List<ThreadAndState> FindWaitingStateThreads(this List<ThreadAndState> threadAndStates)
        {
            return threadAndStates.Where(ThreadAndState => ThreadAndState.ThreadState == Enums.ThreadState.Wait).ToList();
        }

        /// <summary>
        /// Removes all threads that satisfy the given condition and returns the modified list.
        /// </summary>
        /// <param name="threadAndStates">The list of ThreadAndState objects.</param>
        /// <param name="Condition">The condition used to filter which threads to remove.</param>
        /// <param name="Count">Outputs the number of removed threads.</param>
        /// <returns>The modified list of ThreadAndState objects.</returns>
        public static List<ThreadAndState> RemoveWaitingThreads(this List<ThreadAndState> threadAndStates,
            Func<ThreadAndState, bool> Condition, out int Count)
        {
            Count = threadAndStates.Count(Condition);
            threadAndStates.RemoveAll(item => Condition(item));
            return threadAndStates;
        }

    }

}
