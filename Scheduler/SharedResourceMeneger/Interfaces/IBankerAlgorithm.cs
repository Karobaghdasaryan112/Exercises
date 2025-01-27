using Scheduler.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.SharedResourceMeneger.Interfaces
{
    public interface IBankerAlgorithm
    {

        void RegisterResource(ISharedResource sharedResource, int InstanceCount);

        void RegisterProccess(int processIdentifeir, Dictionary<int, int> sharedResources);

        RequestApproval AllocateResource(int resourceIdentifier,Dictionary<int,int> sharedResources);    

        void ReInitializeAllocation(int processIdentifier,Dictionary<int,int> sharedResources);

        void FreeResources(int processIdentifier,Dictionary<int,int> sharedResources);


    }
}
