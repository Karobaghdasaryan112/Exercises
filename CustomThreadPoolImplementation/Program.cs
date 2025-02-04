using System.Diagnostics;
using System.Text;

namespace CustomThreadPoolImplementation
{
    public class Program
    {
        public static void Main()
        {
            CustomThreadPool customThreadPool = new CustomThreadPool();
            string str = "work";
            object obj = new object();
            customThreadPool.QueueUserWorkItem((obj) => { Thread.Sleep(4000); Console.WriteLine(str); });
            customThreadPool.QueueUserWorkItem((obj) => { Thread.Sleep(4000); Console.WriteLine(str); });


            Thread.Sleep(10000);
            Console.WriteLine(customThreadPool.ActiveThreadsCount);
            Console.ReadLine();

        }


    }
}