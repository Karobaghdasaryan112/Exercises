using System.Diagnostics;
using System.Text;

namespace CustomThreadPoolImplementation
{
    public class Program
    {
        public static void Main()
        {
            CustomThreadPool customThreadPool = new CustomThreadPool();
            string str = "karo";
            object obj = new object();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //customThreadPool.SetMaxThreads(10, 2);
            customThreadPool.QueueUserWorkItem((obj) => { Thread.Sleep(4000); Console.WriteLine(str);Console.WriteLine("____"); });
            customThreadPool.QueueUserWorkItem((obj) => { Thread.Sleep(4000); Console.WriteLine(str);});

            //customThreadPool.QueueUserWorkItem((obj) => { Thread.Sleep(4000); Console.WriteLine(str);});
            //customThreadPool.QueueUserWorkItem((obj) => { Thread.Sleep(4000); Console.WriteLine(str);});
            //customThreadPool.QueueUserWorkItem((obj) => { Thread.Sleep(4000); Console.WriteLine(str);});
            //customThreadPool.QueueUserWorkItem((obj) => { Thread.Sleep(4000); Console.WriteLine(str);});
            //customThreadPool.QueueUserWorkItem((obj) => { Thread.Sleep(4000); Console.WriteLine(str); });
            //customThreadPool.QueueUserWorkItem((obj) => { Thread.Sleep(4000); Console.WriteLine(str); });
            //customThreadPool.QueueUserWorkItem((obj) => { Thread.Sleep(4000); Console.WriteLine(str); });
            //customThreadPool.QueueUserWorkItem((obj) => { Thread.Sleep(4000); Console.WriteLine(str); });
            //customThreadPool.QueueUserWorkItem((obj) => { Thread.Sleep(4000); Console.WriteLine(str); });
            //customThreadPool.QueueUserWorkItem((obj) => { Thread.Sleep(4000); Console.WriteLine(str); });
            stopwatch.Stop();
            Thread.Sleep(10000);
            Console.WriteLine(customThreadPool.ActiveThreadsCount);
            Console.WriteLine(stopwatch.ElapsedTicks);
            Console.ReadLine();

        }


    }
}