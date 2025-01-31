using Scheduler.Enums;
using Scheduler.SharedResourceMeneger.Interfaces;
using System.Collections.Concurrent;                                  

namespace Scheduler.SharedResourceMeneger.Services.SchedulerService
{
    public abstract class CoreTaskScheduler : TaskScheduler
    {
        protected IBankerAlgorithm sharedResourceManager => new BankerAlgorithm();

        protected ConcurrentQueue<PrioritizedLimitedTask> pendingTasks = new ConcurrentQueue<PrioritizedLimitedTask>();

        protected readonly int MaxLevelOfParallelizm;

        protected readonly object schedulerLockObject = new object();

        public override int MaximumConcurrencyLevel => this.MaxLevelOfParallelizm;

        public CoreTaskScheduler(int maxLevelOfParallelizm)
        {
            schedulerLockObject = maxLevelOfParallelizm;
        }
        public void QueueForScheduling(IList<PrioritizedLimitedTask> tasksForScheduling)
        {
            tasksForScheduling = tasksForScheduling.OrderByDescending(task => task.Priority, new PriorityComparer()).ToList();
            foreach (PrioritizedLimitedTask TaskWithInfo in tasksForScheduling)
                TaskWithInfo.Start(this);
        }

        public void SortPendingTasks()
        {
            pendingTasks = new ConcurrentQueue<PrioritizedLimitedTask>(
                pendingTasks.AsParallel().
                    WithDegreeOfParallelism(Environment.ProcessorCount).
                    OrderByDescending(x => x.Priority, new PriorityComparer()));
        }

        protected PrioritizedLimitedTask GetNextTaskWithDeadLockAvoidence()
        {
            pendingTasks.TryDequeue(out var TaskWithInformation);
            if (TaskWithInformation.UsesSharedResource)
            {
                RequestApproval approved = sharedResourceManager.AllocateResource(TaskWithInformation.PrioritizedLimitetdTaskIdentifier, TaskWithInformation.SharedResources);

                if (approved == RequestApproval.Wait)
                {
                    PrioritizedLimitedTask NextTask = GetNextTaskWithDeadLockAvoidence();
                    pendingTasks.Enqueue(TaskWithInformation);

                    TaskWithInformation = NextTask;
                }
            }
            return TaskWithInformation;
        }

        public abstract void RunScheduling();

        protected override IEnumerable<Task>? GetScheduledTasks()
        {
            return pendingTasks;
        }


        protected abstract override void QueueTask(Task task);

        //karelia ardyoq Task @ katarel mnuyn pahin te inq@ petqa texavorvi herti mej heto execute linelu hamar
        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false;
        }

        protected void StartCallback(PrioritizedLimitedTask task, Action controlNumberOfExecutionTasksAction, Action enablePauseAction = null)
                   => Task.Factory.StartNew(() =>
                   {
                       Task.Delay(task.DurationInMiliseconds).Wait();
                       enablePauseAction?.Invoke();
                       task.CooperationMechanism.Cancel();
                       // Free shared resources
                       if (task.UsesSharedResource)
                           sharedResourceManager.FreeResources(task.PrioritizedLimitetdTaskIdentifier, task.SharedResources);
                       controlNumberOfExecutionTasksAction.Invoke();
                       RunScheduling();
                   }, CancellationToken.None, TaskCreationOptions.None, Default);

    }
}
