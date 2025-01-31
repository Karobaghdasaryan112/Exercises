using Scheduler.Enums;
using Scheduler.SharedResourceMeneger.Interfaces;


namespace Scheduler.SharedResourceMeneger.Services
{
    public class BankerAlgorithm : IBankerAlgorithm
    {
        private static readonly object BankerAlgorithmLocker = new object();

        //mi qani threadneri koxmic ogtagorcvox resources vor@ petqa DeadLockic xusapelu hamar

        private Dictionary<int, int> avilableSharedResources = new Dictionary<int, int>();

        //kalekcia vor@ pahuma e informacia processneri veraberyal
        private readonly List<int> registeredProcess = new List<int>();


        //Maxmimum pahanjark@ amen mi procesi koxmic pahanjvox amen mi
        //maximumDemandes[1] = new Dictionary<int, int> { { 1, 3 }, { 2, 2 } }; // Процесс 1 может запросить до 3 экземпляров ресурса 1 и до 2 экземпляров ресурса 2
        //maximumDemandes[2] = new Dictionary<int, int> { { 1, 4 }, { 2, 1 } }; // Процесс 2 может запросить до 4 экземпляров ресурса 1 и до 1 экземпляра ресурса 2
        //maximumDemandes[3] = new Dictionary<int, int> { { 1, 2 }, { 2, 3 } }; // Процесс 3 может запросить до 2 экземпляров ресурса 1 и до 3 экземпляров ресурса 2
        private readonly Dictionary<int, Dictionary<int, int>> MaximumDemandes = new Dictionary<int, Dictionary<int, int>>();



        private readonly Dictionary<int, Dictionary<int, int>> allocatedResources = new Dictionary<int, Dictionary<int, int>>();


        /// <summary>
        /// Represents matrix that indicates the remaining resource need of each process.
        /// </summary>
        private readonly Dictionary<int, Dictionary<int, int>> neededResources = new Dictionary<int, Dictionary<int, int>>();

        public RequestApproval AllocateResource(int processIdentifier, Dictionary<int, int> sharedResources)
        {
            lock (BankerAlgorithmLocker)
            {
                static bool greatherThan(int a, int b) => a > b;

                //hamematum ayn resource i het vor@ uzum e process@ ev ayn resource i het vor@ pahanjvum e dra hamar.
                //ete pahanjvox resource @ aveli shat e qan ayn resource @ vor@ petq e processi hamar apa ayn chexarkvum e
                if (sharedResources.SatisfiesCondition(neededResources[processIdentifier], greatherThan))
                    return RequestApproval.Denied;

                ///<summary>
                ///hamematum ayn resource i het vor@ ays pahin azat e mnacel ayn resource i het vor@ vor petq e tvyal process in 
                ///ete mnavcac resourc e ner@ aveli qich en apa petq e ayd proces@ spasi
                ///</summary>
                else if (sharedResources.SatisfiesCondition(avilableSharedResources, greatherThan))
                    return RequestApproval.Wait;

                //pahpanum enq naxkin arjeqner@
                Dictionary<int, int> sharedResourcesOriginal = new Dictionary<int, int>(sharedResources);
                Dictionary<int, int> neededResourcesOriginal = new Dictionary<int, int>(neededResources[processIdentifier]);
                Dictionary<int, int> allocatedResourcesOriginal = new Dictionary<int, int>(allocatedResources[processIdentifier]);

                bool NoDeadLock = IsInSafeState();
                if (!NoDeadLock)
                {
                    avilableSharedResources = sharedResourcesOriginal;
                    allocatedResources[processIdentifier] = allocatedResourcesOriginal;
                    neededResources[processIdentifier] = neededResourcesOriginal;
                    return RequestApproval.Wait;
                }

                return RequestApproval.Approved;

            }
        }
        private bool IsInSafeState()
        {
            Dictionary<int, bool> finished = new Dictionary<int, bool>();
            Dictionary<int, int> Work = new Dictionary<int, int>(avilableSharedResources);

            registeredProcess.ForEach(processIdentifier => finished[processIdentifier] = false);

            for (int processId = 0;

                processId < registeredProcess.Count() &&
                !finished[processId] &&
                neededResources[processId].SatisfiesCondition(Work, (x, y) => x <= y);

                processId++)
            {
                Work.ApplayOperation(avilableSharedResources, (x, y) => x + y);
                finished[processId] = true;
            }

            return finished.Any(process => !process.Value);
        }

        //procesi resourceneri azatum.da nshanakuma vor gumarum enq avilablesharedresource in
        //aysinqn es pahin inch azat resourdce unenq dranc gumarvuma en resourcener@ voronq menq azatel enq

        //pakasuma neededResource neric vorovhetev pahanjvox resourceneri qanak@ qchanuma enqanov inchqan vor process@ azatela resource


        public void FreeResources(int processIdentifier, Dictionary<int, int> sharedResources)
        {
            lock (BankerAlgorithmLocker)
            {
                avilableSharedResources.ApplayOperation(sharedResources, (x, y) => x + y);

                neededResources[processIdentifier].ApplayOperation(sharedResources, (x, y) => x - y);

                if (!neededResources[processIdentifier].AsParallel().Select(resource => resource.Value).Any(need => need != 0))
                {
                    registeredProcess.Remove(processIdentifier);
                    MaximumDemandes.Remove(processIdentifier);
                    neededResources.Remove(processIdentifier);
                    allocatedResources.Remove(processIdentifier);

                }
            }
        }

        public void RegisterProccess(int processIdentifier, Dictionary<int, int> sharedResources)
        {
            lock (BankerAlgorithmLocker)
            {
                registeredProcess.Add(processIdentifier);
                MaximumDemandes.Add(processIdentifier, sharedResources);
                neededResources.Add(processIdentifier, sharedResources);

                int noAllocation = 0;
                Dictionary<int, int> allocatedResources = new Dictionary<int, int>();
                foreach (int resourceIdentifier in sharedResources.Keys)
                    allocatedResources.Add(resourceIdentifier, noAllocation);
                if (this.allocatedResources.ContainsKey(processIdentifier))
                    this.allocatedResources[processIdentifier] = allocatedResources;
                else
                    this.allocatedResources.Add(processIdentifier, allocatedResources);
            }
        }


        public void RegisterResource(ISharedResource sharedResource, int InstanceCount)
        {
            lock (BankerAlgorithmLocker)
            {
                int resourceIdentifier = sharedResource.GetResourceIdentifier();
                if (!avilableSharedResources.ContainsKey(resourceIdentifier))
                    avilableSharedResources.Add(resourceIdentifier, InstanceCount);
                else
                    avilableSharedResources[resourceIdentifier] += InstanceCount;
            }
        }

        public void ReInitializeAllocation(int processIdentifier, Dictionary<int, int> sharedResources)
        {
            lock (BankerAlgorithmLocker)
                if (allocatedResources.ContainsKey(processIdentifier))
                    allocatedResources[processIdentifier] = sharedResources;
        }
    }
    public static class DFictionaryUtil
    {
        public static bool SatisfiesCondition(this Dictionary<int, int> original, Dictionary<int, int> request, Func<int, int, bool> Condition)
        {
            foreach (var key in request.Keys)
                if (!Condition.Invoke(original[key], request[key]))
                    return false;

            return true;
        }
        public static void ApplayOperation(this Dictionary<int, int> original, Dictionary<int, int> modifier, Func<int, int, int> operation)
        {
            //i think this is a wrong Code
            //Dictionary<int, int> result = new Dictionary<int, int>(original);
            foreach (int resourceKey in original.Keys)
                original[resourceKey] = operation.Invoke(original[resourceKey], modifier[resourceKey]);
        }
    }
}
