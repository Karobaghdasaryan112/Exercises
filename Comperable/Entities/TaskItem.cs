using Comperable.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comperable.Entities
{
    public class TaskItem : IComparable<TaskItem>
    {
        public PriorityEnum Priority { get; set; }
        public DateTime DueData { get; set; }
        public string? Title { get; set; }
        public TaskItem(PriorityEnum priority, DateTime dueData, string? title)
        {
            Priority = priority;
            DueData = dueData;
            Title = title;
        }

        public int CompareTo(TaskItem? other)
        {
            if (other == null) return 1;


            //if they have the same Reference
            if (this == other) return 0;

            if (other.Priority == this.Priority)
            {
                return other.DueData.CompareTo(this.DueData);
            }
            else
            {
                return ((int)this.Priority).CompareTo(((int)other.Priority));
            }
        }
    }
}
