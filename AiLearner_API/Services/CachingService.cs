using Microsoft.Extensions.Caching.Memory;

namespace AiLearner_API.Services
{
    // A service to cache objects in memory
    public class CachingService(IMemoryCache memoryCache)
    {
        private readonly IMemoryCache _memoryCache = memoryCache;

        // Try to get a cached item by its key
        public bool TryGetCachedItem<T>(string key,out T? value)
        {
            string uniuqKey = ConstructCacheKey(key, typeof(T));


            return _memoryCache.TryGetValue<T>(uniuqKey, out value);
        }

        public void CacheItem<T>(string key, T value)
        {
            string uniuqKey = ConstructCacheKey(key, typeof(T));


            var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10)) 
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1)); 

            _memoryCache.Set(uniuqKey, value, cacheEntryOptions);
        }

        // Construct a unique cache key based on the user id and the type of the object
        private string ConstructCacheKey(string userId, Type type)
        {
            string typeName = type.Name;

            if (type.IsGenericType)
            {
                // Get the generic type definition (e.g., List<>)
                var genericTypeDefinition = type.GetGenericTypeDefinition().Name;
                
                // Clean up the name by removing the `1, `2 etc.
                genericTypeDefinition = genericTypeDefinition.Substring(0, genericTypeDefinition.IndexOf('`'));

                // Get the generic arguments (Material, Question...)
                var genericArguments = type.GetGenericArguments().Select(t => t.Name).ToArray();

                // Construct a readable type name (List<Material>...)
                typeName = $"{genericTypeDefinition}<{string.Join(", ", genericArguments)}>";
            }

            return $"{userId}{{{typeName}}}";
        }
    }
}
