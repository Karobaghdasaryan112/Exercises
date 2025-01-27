

namespace Disposable.CompositeDispose
{
    public class ResourceMeneger : IDisposable
    {
        private List<IDisposable> _resources = new();

        private bool _disposed;

        public void AddResource(IDisposable resource)
        {
            if (resource == null)
                throw new ArgumentNullException(nameof(resource), "resource cannot be null");


            _resources.Add(resource);
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            List<Exception> exceptions = new List<Exception>();
            foreach (IDisposable resource in _resources)
            {
                try
                {
                    resource.Dispose();
                }
                catch (Exception ex)
                {

                    exceptions.Add(ex);
                }
            }
            _resources.Clear();

            if (exceptions.Count > 0)
            {
                throw new AggregateException("One or more resources failed to dispose.", exceptions.ToArray());
            }
        }

    }
}
