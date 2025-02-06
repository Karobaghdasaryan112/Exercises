using CustomThreadSafeCache.Interfaces;

namespace CustomThreadSafeCache.Service
{
    public class CachingService : ICachingService
    {
        // Dictionary to store cached objects, using a string key
        private Dictionary<string, object> _cache = new Dictionary<string, object>();

        // Dictionary to store cache times for each key, using a string key
        private Dictionary<string, DateTime> _cacheTime = new Dictionary<string, DateTime>();

        //Maximum Minutes(CacheTime)
        private const int DURATION = 20;

        public CachingService()
        {
            // Constructor for initializing the caching service
        }

        // Check if a specific key exists in the cache
        public Task<bool> Exist(string key)
        {
            // Returns true if the key is found in the cache, false otherwise
            return Task.FromResult(_cache.ContainsKey(key));
        }

        // Retrieve the cache time for a specific key
        public Task<DateTime> GetCacheTime(string key)
        {
            // If the cache time for the key exists, return it, otherwise return DateTime.MinValue
            if (_cacheTime.ContainsKey(key))
                return Task.FromResult(_cacheTime[key]);

            // Return the minimum value if the key does not exist in the cache
            return Task.FromResult(DateTime.MinValue);
        }

        // Get an entity from the cache or set it if not already present
        public Task<object> GetOrSet<TResult>(string key, IEntity entity)
        {

            // If the key already exists in the cache, return the cached entity

            UpdateCachingDateOrRemoveCache(key);

            // If not in cache, create a unique key for the entity using its ID
            key = $"{typeof(TResult).Name}:{entity.Id}";

            // Add the entity to the cache and store the current time for cache expiration
            _cache[key] = entity;

            _cacheTime[key] = DateTime.Now;

            // Return the newly added entity
            return Task.FromResult((object)entity);
        }

        // Get a list of entities from the cache or set it if not already present
        public Task<object> GetOrSet<TResult>(string key, List<object> entities)
        {
            // If the key already exists in the cache, return the cached list of entities
            UpdateCachingDateOrRemoveCache(key);

            // For a list, use a generic key name such as the type of entities
            key = $"{nameof(entities)}";

            // Add the list to the cache and store the current time for cache expiration
            _cache[key] = entities;

            _cacheTime[key] = DateTime.Now;

            // Return the newly added list of entities
            return Task.FromResult((object)entities);
        }

        // Try to retrieve a value from the cache, returning true if successful
        public Task<bool> TryGetValue<TResult>(string key, out object result)
        {
            UpdateCachingDateOrRemoveCache(key);
            // Try to get the value for the key, return true if found, false otherwise
            return Task.FromResult(_cache.TryGetValue(key, out result));
        }



        public void UpdateCachingDateOrRemoveCache(string key)
        {
            if (_cache.ContainsKey(key))
            {
                if (_cacheTime[key].AddSeconds(DURATION) > DateTime.UtcNow)
                {
                    _cacheTime[key] = DateTime.UtcNow;
                    Console.WriteLine("Update Cahching Time");
                }
                else
                {
                    Console.WriteLine("Time of Cache is outdated");
                    _cache.Remove(key);
                    _cacheTime.Remove(key);
                }
            }
        }
    }

}
