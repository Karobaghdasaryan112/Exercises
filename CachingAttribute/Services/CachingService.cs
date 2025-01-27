using CachingAttribute.Datas;
using CachingAttribute.Interfaces;


namespace CachingAttribute.Services
{
    public class CachingService : IChaching
    {
        private CachingData _data;
        public CachingService(CachingData data)
        {
            _data = data;
        }
        public object GetOrSet(string cacheKey, object result)
        {
            if (string.IsNullOrWhiteSpace(cacheKey))
                throw new ArgumentException("Cache key cannot be null or empty.", nameof(cacheKey));

            if (_data.Get(cacheKey, out var cachedResult))
            {
                return cachedResult;
            }

            _data.Set(cacheKey, result);
            return result;
        }
        public bool TryGetValue(string ChachKey, out object Value)
        {
            if (string.IsNullOrEmpty(ChachKey))
            {
                Value = null;
                return false;
            }
            return _data.Get(ChachKey, out Value);
                

        }
    }
}
