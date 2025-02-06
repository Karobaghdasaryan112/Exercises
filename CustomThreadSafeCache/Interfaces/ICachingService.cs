namespace CustomThreadSafeCache.Interfaces
{
    public interface ICachingService
    {
        /// <summary>
        /// get Value From Caching Service if it Exist And Set it into Caching Service if Don't
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<object> GetOrSet<TResult>(string key, IEntity entity);

        Task<object> GetOrSet<TResult>(string key, List<object> entities);

        /// <summary>
        /// tryGet Data From CahcingService if it Exist And return true ,otherwise return false
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public Task<bool> TryGetValue<TResult>(string key, out object result);

        /// <summary>
        /// if the cache is Exist return true else return false
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> Exist(string key);

        /// <summary>
        /// get the Caching time (Time To Live)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<DateTime> GetCacheTime(string key);
    }
}
