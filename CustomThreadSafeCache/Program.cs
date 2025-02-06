using CustomThreadSafeCache.Datas;
using CustomThreadSafeCache.Entities;
using CustomThreadSafeCache.Service;
using System.Diagnostics;

namespace CustomChachingService
{
    public class Program 
    {
        public static  void Main(string[] args)
        {
            SetIntoFile.GetInstance();

            Task.FromResult(GetUser());
            Console.ReadLine();
        }
        public static async Task GetUser()
        {
            DataService<User> dataService = new DataService<User>();
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            var User = await dataService.GetEntityById(1);
            Console.WriteLine(((User)User).Name);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedTicks);

            Stopwatch stopwatchForCache = Stopwatch.StartNew();
            stopwatchForCache.Start();
            var UserCache = await dataService.GetEntityById(1);
            stopwatchForCache.Stop();
            Console.WriteLine(stopwatchForCache.ElapsedTicks);

            Thread.Sleep(20000);
            //After 20 seconds
            Stopwatch stopwatchnew = Stopwatch.StartNew();
            stopwatchnew.Start();
            var Usernew = await dataService.GetEntityById(1);
            stopwatchForCache.Stop();
            Console.WriteLine(stopwatchnew.ElapsedTicks);
            Console.ReadLine();
        }
    }
   
}
