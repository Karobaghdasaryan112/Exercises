using ProxcyService.Implementation;
using ProxcyService.ProxyService;
using ProxcyService.Services;
using System.Reflection;

namespace ProxyService
{
    public class Program 
    {
        public static void Main(string[] args)
        {
            IService service = new Service();

            // Create the proxy
            var proxy = DispatchProxy.Create<IService, LoggingProxy<IService>>();
            ((LoggingProxy<IService>)proxy).SetService(service);    
            proxy.DoOtherWor();
            proxy.DoWork();
            Console.ReadLine(); 
        }
    }
}
