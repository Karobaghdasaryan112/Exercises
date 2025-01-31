using Scheduler.Enums;


namespace Scheduler.SharedResourceMeneger.Services.SchedulerService
{
    public class PrioritizedLimitedTask : Task, IComparable<PrioritizedLimitedTask>
    {
        private static int _nextId = 1;
        private const int miminumIdentifierValue = 28;
        private const int maximumIdentifierValue = 1997;

        public CooperationMechanizm CooperationMechanism { get; set; }
        public Dictionary<int, int> SharedResources { get; set; } = new Dictionary<int, int>();
        /// <summary>
        /// Maximum time to execution Task in Miliseconds
        /// </summary>
        public int DurationInMiliseconds { get; set; }
        public int PrioritizedLimitetdTaskIdentifier { get; set; }
        public Action Action { get; set; }
        public bool UsesSharedResource => SharedResources.Count != 0;

        public Priority Priority { get; set; }
        public PrioritizedLimitedTask(Action action, Priority priority, int durationInMiliseconds) : base(action)
        {
            Action = action;
            Priority = priority;
            DurationInMiliseconds = durationInMiliseconds;
        }

        public int CompareTo(PrioritizedLimitedTask? other)
        {
            return Priority.CompareTo(other?.Priority);
        }
    }
}
