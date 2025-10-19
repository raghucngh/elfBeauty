using elfBeauty.Core.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace elfBeauty.Infra.Persistence
{
    public class MemoryCache<T> where T : class
    {
        public IEnumerable<T> SetBrewery(IMemoryCache cache)
        {
            if (!cache.TryGetValue(Const.CacheKey, out IEnumerable<T> collection))
            {
                cache.Set(Const.CacheKey, collection, new MemoryCacheEntryOptions().SetSlidingExpiration(Const.BrewerySyncInterval));
            }

            return collection;
        }
    }
}
