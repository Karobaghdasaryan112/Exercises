using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.SharedResourceMeneger.Interfaces
{
    public interface ISharedResource
    {
        /// <summary>
        /// thris Identifier is level of class 
        /// </summary>
        /// <returns></returns>
        
        int GetResourceIdentifier(); 
    }
}
