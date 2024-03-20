using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace DistributedSystem.Repository
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IDistributedCache _distributedCache;

        public CacheRepository(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T?> GetData<T>(string id, CancellationToken cancellation = default) where T : class
        {
            var value = await _distributedCache.GetStringAsync(id, cancellation);

            if (!string.IsNullOrEmpty(value))
            {
                return JsonConvert.DeserializeObject<T>(value);
            }

            return null;
        }

        public async Task RemoveData(string id, CancellationToken cancellation = default)
        {
            await _distributedCache.RemoveAsync(id, cancellation);
        }

        public async Task SetData<T>(string id, T value, CancellationToken cancellation = default)
        {
            string cacheValue = JsonConvert.SerializeObject(value);
            await _distributedCache.SetStringAsync(id, cacheValue, cancellation);
        }
    }
}
