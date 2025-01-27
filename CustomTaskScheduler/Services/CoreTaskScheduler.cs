using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomTaskScheduler.Services
{
    public class CoreTaskScheduler : TaskScheduler
    {
        protected ConcurrentQueue<PrioritizedLimitedTask> pendingTasks = new ConcurrentQueue<PrioritizedLimitedTask>();

        protected readonly int maxLevelOfParralellism;


        protected readonly object schedulingLocker = new object();

        public override int MaximumConcurrencyLevel => maxLevelOfParralellism;


        public CoreTaskScheduler(int maxLevelOfParralellism)
        {
            this.maxLevelOfParralellism = maxLevelOfParralellism;
        }

        public void QueueForScheduling(IList<PrioritizedLimitedTask> TasksForScheduling)
        {

        }

        //Implementation an apstract Class (Task)
        protected override IEnumerable<Task>? GetScheduledTasks()
        {
            throw new NotImplementedException();
        }

        protected override void QueueTask(Task task)
        {
            throw new NotImplementedException();
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            throw new NotImplementedException();
        }
        //End of Implementation


    }
}
