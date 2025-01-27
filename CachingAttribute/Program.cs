using CachingAttribute.CacheData;
using CachingAttribute.Datas;
using CachingAttribute.Interfaces;
using CachingAttribute.Services;
using System.Reflection;

namespace CachingAttribute
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CachingData cachingData = new CachingData();
            CachingService cachingService = new CachingService(cachingData);
            DataService dataService = new DataService();

            var proxyService = DispatchProxy.Create<IDataService, ProxyService<IDataService>>();

            ((ProxyService<IDataService>)proxyService).SetInstance(dataService);
            ((ProxyService<IDataService>)proxyService).SetDatas(cachingService);

            proxyService.GetData(10);
            proxyService.GetData(10);   
            Console.ReadLine();
        }
    }
}
