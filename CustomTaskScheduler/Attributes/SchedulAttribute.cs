using CustomTaskScheduler.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomTaskScheduler.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SchedulAttribute : Attribute
    {
        public PriorityEnum Priority {  get; set; } 
        public SchedulAttribute(PriorityEnum priority)
        {
            Priority = priority;
        }

    }

}
