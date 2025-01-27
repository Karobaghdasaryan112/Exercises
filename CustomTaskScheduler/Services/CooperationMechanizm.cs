using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomTaskScheduler.Services
{
    /// <summary>
    /// Mechanizm for cooperative Task Canceling And ContextSwithcing
    /// </summary>
    public class CooperationMechanizm
    {
        /// <summary>
        /// Indicates whether the Task can be Canceled 
        /// </summary>
        public bool isCanceled {  get; set; }
        /// <summary>
        /// Indicates whether the Task can be Poused(using for Context switching)
        /// </summary>
        public bool CanPaused { get; set; }

        /// <summary>
        /// Indicates whether the Task is Paused
        /// </summary>

        public bool IsPaused { get; set; }

        /// <summary>
        /// Indicates the total time that Task has been poused for 
        /// </summary>
        public int PausedFor {  get; set; }


        /// <summary>
        /// Task can be Cancelled cooperatively
        /// </summary>
        public void Cancel() => isCanceled = true;

        /// <summary>
        /// Task should be paused cooperatively.
        /// </summary>
        /// <param name="PauseTime"></param>
        public void Pause(int PauseTime)
        {
            IsPaused = true;
            PausedFor += PauseTime;
        }
    }
}
