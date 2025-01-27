using ProxcyService.Attributes;
using System.Reflection;

namespace ProxcyService.ProxyService
{
    public class LoggingProxy<T> : DispatchProxy
    {
        private T  _service;

        public void SetService(T service)
        {
            _service = service;
        }
        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            var LoggingAttribute = targetMethod.GetCustomAttribute<LoggingExecutionAttribute>();

            if (LoggingAttribute != null)
            {
                Console.WriteLine($"[LoggingExecution] starting work: {targetMethod.Name} in {DateTime.Now}");
                DateTime startTime = DateTime.Now;
                var result = targetMethod.Invoke(_service, args ?? Array.Empty<object>());

                DateTime EndTine = DateTime.Now;
                Console.WriteLine($"[LoggingExecution] ending work: {targetMethod.Name} in {DateTime.Now}");
                Console.WriteLine($"this work is doing in {EndTine - startTime} Time");
                return result;
            }
            return targetMethod.Invoke(_service, args ?? Array.Empty<object>());
        }
    }
}
