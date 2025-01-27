using CachingAttribute.Attributes;
using CachingAttribute.Services;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

public class ProxyService<T> : DispatchProxy
{
    private T _service;

    private CachingService _cachingService;

    public void SetInstance(T service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    public void SetDatas(CachingService cachingService)
    {
        _cachingService = cachingService ?? throw new ArgumentNullException(nameof(cachingService));
    }

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        if (targetMethod == null)
            throw new ArgumentNullException(nameof(targetMethod));

        var cacheAttribute = targetMethod.GetCustomAttribute<CacheResultAttribute>();
        if (cacheAttribute != null)
        {

            string argsKey = args != null ? string.Join(", ", args.Select(arg => arg?.ToString() ?? "null")) : "NoArgs";
            string cacheKey = $"{targetMethod.Name}({argsKey})";

            Console.WriteLine($"[Cache] Checking key: {cacheKey}");


            var stopwatch = Stopwatch.StartNew();

            if (_cachingService.TryGetValue(cacheKey, out object cachedResult))
            {
                stopwatch.Stop();
                Console.WriteLine($"[Cache] Hit! Retrieved in {stopwatch.ElapsedMilliseconds} ms.");
                return cachedResult;
            }

            Console.WriteLine("[Cache] Miss. Invoking method and caching result...");


            var result = targetMethod.Invoke(_service, args);
            _cachingService.GetOrSet(cacheKey, result);

            stopwatch.Stop();
            Console.WriteLine($"[Cache] Stored result in {stopwatch.ElapsedMilliseconds} ms.");
            return result;
        }


        return targetMethod.Invoke(_service, args);
    }
}
