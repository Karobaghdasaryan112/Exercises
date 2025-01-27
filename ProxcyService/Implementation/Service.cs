using ProxcyService.Attributes;
using ProxcyService.Services;

namespace ProxcyService.Implementation
{
    public class Service : IService
    {

        public void DoOtherWor()
        {

            Console.WriteLine("Doing Other work...");
            Thread.Sleep(1000);
        }

        public void DoWork()
        {

            Console.WriteLine("Doing work...");
            Thread.Sleep(1000);
        }
    }
}
