using CustomTaskScheduler.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomTaskScheduler.Services
{
    public class PrioritizedLimitedTask : Task, IComparable<PrioritizedLimitedTask>
    {
        private const int MinimumIdentifierValue = 28;
        private const int MaximumIdentifierValue = 1997;
        public PriorityEnum Priority { get; set; }

        public int PrioritizedLimitedTaskIdentifier {  get; set; }

        public Action Action { get; set; }

        public int DurationInMiliseconds {  get; set; }


        public Dictionary<int, int> SharedResources { get; set; } = new Dictionary<int, int>();


        public PrioritizedLimitedTask(Action action, PriorityEnum priorityEnum, int durationInMiliseconds) : base(action)
        {
            Action = action;

            Priority = priorityEnum;

            DurationInMiliseconds = durationInMiliseconds;

            PrioritizedLimitedTaskIdentifier = new Random().Next(MinimumIdentifierValue, MaximumIdentifierValue);
        }

        public int CompareTo(PrioritizedLimitedTask? other)
        {
            return Priority.CompareTo(other.Priority);
        }

        public bool UsesSharedResource => SharedResources.Count != 0;

    }
}
