using Scheduler.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.SharedResourceMeneger.Services.SchedulerService
{
    public class PriorityComparer : IComparer<Priority>
    {
        public int Compare([AllowNull] Priority x, [AllowNull] Priority y)
        {
            return x.CompareTo(y); 
        }
    }
}
