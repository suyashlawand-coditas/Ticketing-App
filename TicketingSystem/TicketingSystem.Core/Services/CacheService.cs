using StackExchange.Redis;
using System.Text.Json;
using TicketingSystem.Core.ServiceContracts;

namespace TicketingSystem.Core.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _cacheDb;

        public CacheService(string connectionString) {
            _cacheDb = ConnectionMultiplexer.Connect(connectionString).GetDatabase();
        }

        public async Task Delete(string key)
        {
            await _cacheDb.KeyDeleteAsync(key);
        }

        public async Task<bool> DoesExist(string key)
        {
            return await _cacheDb.KeyExistsAsync(key);
        }

        public async Task<string?> Get(string key)
        {
            return await _cacheDb.StringGetAsync(key);
        }

        public async Task Set(string key, object value, TimeSpan? timeSpan = null)
        {
            if (value is string valueAsString)
            {
                await _cacheDb.StringSetAsync(key, valueAsString, timeSpan);
            } else
            {
                await _cacheDb.StringSetAsync(key, JsonSerializer.Serialize(value), timeSpan);
            }
        }
    }
}
