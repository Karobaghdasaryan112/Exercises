using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Disposable.AsyncDispose
{
    public class AsyncOperations : IAsyncDisposable, IDisposable
    {
        private readonly HttpClient _httpClient;
        private  bool _disposed;
        public AsyncOperations()
        {
            _httpClient = new HttpClient();
        }
        public async Task<string> FetchDataAsync(string SomeUrl)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(AsyncOperations));

            if (string.IsNullOrWhiteSpace(SomeUrl))
                throw new ArgumentNullException(nameof(SomeUrl), "Url cannot be null");

            var response = await _httpClient.GetAsync(SomeUrl);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();

            return data;
        }
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            Console.WriteLine("Disposing Resource synchronusly");
            _httpClient.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed)
                return;

            _disposed = true;

            await Task.Delay(1000);
            _httpClient.Dispose();
        }

    }
}
