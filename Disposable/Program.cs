using Disposable.AsyncDispose;
using Disposable.CompositeDispose;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Net.Sockets;

namespace Dispose
{
    public class Program
    {
        public static async void Main(string[] args)
        {
            try
            {

                //Try finnaly Block
                using (ResourceMeneger resourceMeneger = new ResourceMeneger())
                {
                    resourceMeneger.AddResource(new FileStream("someFilePath", FileMode.Open));
                    resourceMeneger.AddResource(new TcpClient("example.com", 8800));
                    resourceMeneger.AddResource(new SqlConnection("someConnectionstring"));
                }

            }
            catch (AggregateException AgrEx)
            {
                Console.WriteLine("Some resources failed to dispose:");

                foreach (var InnarException in AgrEx.InnerExceptions)
                {
                    Console.WriteLine($"{InnarException.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error Occurred: {ex.Message}");
            }


            string someUrl = "https://jsonplaceholder.typicode.com/todos/1";

            await DisposingAsyncResource(someUrl);
        }

        public static async Task DisposingAsyncResource(string SomeUrl)
        {
            await using (var resource = new AsyncOperations())
            {
                var result = await resource.FetchDataAsync(SomeUrl);
                Console.WriteLine($"Data Fetched {result}");
            }
        }
    }
}
