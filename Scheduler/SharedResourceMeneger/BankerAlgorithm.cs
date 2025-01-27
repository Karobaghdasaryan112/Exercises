using Scheduler.Enums;
using Scheduler.SharedResourceMeneger.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.SharedResourceMeneger
{
    public class BankerAlgorithm : IBankerAlgorithm
    {
        private static readonly object BankerAlgorithmLocker = new object(); 

        //mi qani threadneri koxmic ogtagorcvox resources vor@ petqa DeadLockic xusapelu hamar

        private Dictionary<int,int> avliableSharedResources = new Dictionary<int,int>();

        //kalekcia vor@ pahuma e informacia processneri veraberyal
        private readonly List<int> registeredProcess = new List<int>();


        //Maxmimum pahanjark@ amen mi procesi koxmic pahanjvox amen mi
        //maximumDemandes[1] = new Dictionary<int, int> { { 1, 3 }, { 2, 2 } }; // Процесс 1 может запросить до 3 экземпляров ресурса 1 и до 2 экземпляров ресурса 2
        //maximumDemandes[2] = new Dictionary<int, int> { { 1, 4 }, { 2, 1 } }; // Процесс 2 может запросить до 4 экземпляров ресурса 1 и до 1 экземпляра ресурса 2
        //maximumDemandes[3] = new Dictionary<int, int> { { 1, 2 }, { 2, 3 } }; // Процесс 3 может запросить до 2 экземпляров ресурса 1 и до 3 экземпляров ресурса 2
        private readonly Dictionary<int,Dictionary<int,int>> MaximumDemandes = new Dictionary<int, Dictionary<int, int>>();


        private readonly Dictionary<int,Dictionary<int,int>> allocatedResources = new Dictionary<int, Dictionary<int, int>>();


        /// <summary>
        /// Represents matrix that indicates the remaining resource need of each process.
        /// </summary>
        private readonly Dictionary<int, Dictionary<int, int>> neededResources = new Dictionary<int, Dictionary<int, int>>();

        public RequestApproval AllocateResource(int processIdentifier, Dictionary<int, int> sharedResources)
        {
            lock (BankerAlgorithmLocker)
            {
                static bool greatherThan(int a, int b) => a > b;
                if (sharedResources.SatisfiesCondition(neededResources[processIdentifier], (neededResources[processIdentifier], sharedResources[processIdentifier]) => neededResources[processIdentifier] > sharedResources[processIdentifier]))
            }
        }

        public void FreeResources(int processIdentifier, Dictionary<int, int> sharedResources)
        {
            throw new NotImplementedException();
        }

        public void RegisterProccess(int processIdentifeir, Dictionary<int, int> sharedResources)
        {
            throw new NotImplementedException();
        }

        public void RegisterResource(ISharedResource sharedResource, int InstanceCount)
        {
            throw new NotImplementedException();
        }

        public void ReInitializeAllocation(int processIdentifier, Dictionary<int, int> sharedResources)
        {
            throw new NotImplementedException();
        }
    }
    public static class DFictionaryUtil
    {
        public static bool SatisfiesCondition(this Dictionary<int, int> original, Dictionary<int, int> request, Func<int, int, bool> conditionFunc)
        {
            foreach (int Key in request.Keys)
                if (!conditionFunc.Invoke(original[Key], request[Key]))
                    return false;
            return true;
        }
        public static void ApplayOperation(this Dictionary<int,int> original,Dictionary<int,int> modifier,Func<int,int,int> operation)
        {
            Dictionary<int, int> result = new Dictionary<int, int>(original);
            foreach (int resourceKey in original.Keys)
                result[resourceKey] = operation.Invoke(original[resourceKey], modifier[resourceKey]);
        }
    }
}
